using FastEndpoints;

using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

using reymani_web_api.Data;
using reymani_web_api.Endpoints.Business.Requests;
using reymani_web_api.Endpoints.Mappers;
using reymani_web_api.Services.BlobServices;

namespace reymani_web_api.Endpoints.Business
{
  public class UpdateBusinessEndpoint : Endpoint<UpdateBusinessRequest, Results<Ok, NotFound, Conflict, UnauthorizedHttpResult, ForbidHttpResult, ProblemDetails>>
  {
    private readonly AppDbContext _dbContext;
    private readonly IBlobService _blobService;

    public UpdateBusinessEndpoint(AppDbContext dbContext, IBlobService blobService)
    {
      _dbContext = dbContext;
      _blobService = blobService;
    }

    public override void Configure()
    {
      Put("/business/{id}");
      Summary(s =>
      {
        s.Summary = "Update business";
        s.Description = "Updates details of an existing business.";
      });
      Roles("SystemAdmin");
      AllowFormData();
    }

    public override async Task<Results<Ok, NotFound, Conflict, UnauthorizedHttpResult, ForbidHttpResult, ProblemDetails>> ExecuteAsync(UpdateBusinessRequest req, CancellationToken ct)
    {
      // Verificar si ya existe otro negocio con el mismo nombre
      var existingBusiness = await _dbContext.Businesses.FirstOrDefaultAsync(x => x.Name == req.Name, ct);
      if (existingBusiness != null && existingBusiness.Id != req.Id)
      {
        return TypedResults.Conflict();
      }

      // Buscar el negocio por Id
      var business = await _dbContext.Businesses.FindAsync(new object?[] { req.Id }, ct);
      if (business is null)
        return TypedResults.NotFound();

      // Nuevas validaciones de usuario y municipio
      var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == req.UserId, ct);
      if (user == null || user.Role != Data.Models.UserRole.BusinessAdmin)
      {
        AddError(req => req.UserId, "Usuario no encontrado o no es un administrador de negocio.");
      }

      var municipality = await _dbContext.Municipalities
                            .Include(x => x.Province)
                            .FirstOrDefaultAsync(x => x.Id == req.MunicipalityId, ct);
      if (municipality == null)
      {
        AddError(req => req.MunicipalityId, "Municipio no encontrado.");
      }

      ThrowIfAnyErrors();

      // Si se actualiza la imagen del Logo, almacenarla
      if (req.Logo != null)
      {
        string logoCode = Guid.NewGuid().ToString();
        string logoPath = await _blobService.UploadObject(req.Logo, logoCode, ct);
        business.Logo = logoPath;
      }

      // Si se actualiza la imagen del Banner, almacenarla
      if (req.Banner != null)
      {
        string bannerCode = Guid.NewGuid().ToString();
        string bannerPath = await _blobService.UploadObject(req.Banner, bannerCode, ct);
        business.Banner = bannerPath;
      }

      // Actualizar el resto de propiedades usando el mapper
      var mapper = new BusinessMapper();
      business = mapper.ToEntity(req, business);

      // Enlazar usuario y municipio al negocio
      business.User = user;
      business.Municipality = municipality;

      await _dbContext.SaveChangesAsync(ct);
      return TypedResults.Ok();
    }
  }
}
