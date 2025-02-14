using FastEndpoints;

using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

using reymani_web_api.Data;
using reymani_web_api.Endpoints.Business.Requests;
using reymani_web_api.Endpoints.Mappers;
using reymani_web_api.Services.BlobServices;

namespace reymani_web_api.Endpoints.Business
{
  public class UpdateMyBusinessEndpoint : Endpoint<UpdateMyBusinessRequest, Results<Ok, NotFound, Conflict, UnauthorizedHttpResult, ForbidHttpResult, ProblemDetails>>
  {
    private readonly AppDbContext _dbContext;
    private readonly IBlobService _blobService;

    public UpdateMyBusinessEndpoint(AppDbContext dbContext, IBlobService blobService)
    {
      _dbContext = dbContext;
      _blobService = blobService;
    }

    public override void Configure()
    {
      Put("/business/mine");
      Summary(s =>
      {
        s.Summary = "Update my business";
        s.Description = "Updates details of the business managed by the logged in BusinessAdmin.";
      });
      Roles("BusinessAdmin");
      AllowFormData();
    }

    public override async Task<Results<Ok, NotFound, Conflict, UnauthorizedHttpResult, ForbidHttpResult, ProblemDetails>> ExecuteAsync(UpdateMyBusinessRequest req, CancellationToken ct)
    {
      // Obtener id del token JWT
      var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "Id");
      if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
      {
        return TypedResults.Unauthorized();
      }

      // Verificar si ya existe otro negocio con el mismo nombre
      var existingBusiness = await _dbContext.Businesses.FirstOrDefaultAsync(x => x.Name == req.Name, ct);

      // Buscar el negocio asociado al usuario logueado y activo
      var business = await _dbContext.Businesses.FirstOrDefaultAsync(b => b.UserId == userId && b.IsActive, ct);
      if (business is null)
        return TypedResults.NotFound();

      if (existingBusiness != null && existingBusiness.Id != business.Id)
      {
        return TypedResults.Conflict();
      }

      // Validar municipio y otras propiedades
      var municipality = await _dbContext.Municipalities
                            .Include(x => x.Province)
                            .FirstOrDefaultAsync(x => x.Id == req.MunicipalityId, ct);
      if (municipality == null)
      {
        AddError(req => req.MunicipalityId, "Municipio no encontrado.");
      }

      ThrowIfAnyErrors();

      // Actualizar imagen del Logo si se incluye
      if (req.Logo != null)
      {
        string logoCode = Guid.NewGuid().ToString();
        string logoPath = await _blobService.UploadObject(req.Logo, logoCode, ct);
        business.Logo = logoPath;
      }

      // Actualizar imagen del Banner si se incluye
      if (req.Banner != null)
      {
        string bannerCode = Guid.NewGuid().ToString();
        string bannerPath = await _blobService.UploadObject(req.Banner, bannerCode, ct);
        business.Banner = bannerPath;
      }

      // Actualizar el resto de propiedades usando el mapper
      var mapper = new BusinessMapper();
      business = mapper.ToEntity(req, business);

      // Enlazar el municipio al negocio
      business.Municipality = municipality;

      await _dbContext.SaveChangesAsync(ct);
      return TypedResults.Ok();
    }
  }
}
