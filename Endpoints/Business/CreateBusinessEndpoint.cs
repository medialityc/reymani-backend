using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using reymani_web_api.Data;
using reymani_web_api.Endpoints.Business.Requests;
using reymani_web_api.Services.BlobServices;
using reymani_web_api.Endpoints.Business.Responses;
using reymani_web_api.Endpoints.Mappers;

namespace reymani_web_api.Endpoints.Business
{
  public class CreateBusinessEndpoint : Endpoint<CreateBusinessRequest, Results<Created<BusinessSystemAdminResponse>, Conflict, ProblemDetails>>
  {
    private readonly AppDbContext _dbContext;
    private readonly IBlobService _blobService;

    public CreateBusinessEndpoint(AppDbContext dbContext, IBlobService blobService)
    {
      _dbContext = dbContext;
      _blobService = blobService;
    }

    public override void Configure()
    {
      Post("/business");
      Summary(s =>
      {
        s.Summary = "Create business";
        s.Description = "Creates a new business.";
      });
      AllowFormData();
      Roles("SystemAdmin");
    }

    public override async Task<Results<Created<BusinessSystemAdminResponse>, Conflict, ProblemDetails>> ExecuteAsync(CreateBusinessRequest req, CancellationToken ct)
    {
      var existingBusiness = await _dbContext.Businesses.FirstOrDefaultAsync(x => x.Name == req.Name, ct);
      if (existingBusiness != null)
      {
        return TypedResults.Conflict();
      }

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

      var mapper = new BusinessMapper();
      var business = mapper.ToEntity(req);

      if (req.Logo != null)
      {
        string fileCode = Guid.NewGuid().ToString();
        string objectPath = await _blobService.UploadObject(req.Logo, fileCode, ct);
        business.Logo = objectPath;
      }

      if (req.Banner != null)
      {
        string fileCode = Guid.NewGuid().ToString();
        string objectPath = await _blobService.UploadObject(req.Banner, fileCode, ct);
        business.Banner = objectPath;
      }

      business.Municipality = municipality;
      business.User = user;

      _dbContext.Businesses.Add(business);
      await _dbContext.SaveChangesAsync(ct);

      var response = mapper.FromEntity(business);
      if (!string.IsNullOrEmpty(business.Logo))
      {
        response.Logo = await _blobService.PresignedGetUrl(business.Logo, ct);
      }

      if (!string.IsNullOrEmpty(business.Banner))
      {
        response.Banner = await _blobService.PresignedGetUrl(business.Banner, ct);
      }

      return TypedResults.Created($"/businesses/{business.Id}", response);
    }
  }
}
