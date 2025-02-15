using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

using reymani_web_api.Data;

using reymani_web_api.Endpoints.Mappers;
using reymani_web_api.Endpoints.Provinces.Requests;
using reymani_web_api.Endpoints.Provinces.Responses;

using reymani_web_api.Services.BlobServices;

namespace reymani_web_api.Endpoints.Provinces;

public class CreateProvinceEndpoint : Endpoint<CreateProvinceRequest, Results<Created<ProvinceResponse>, Conflict, UnauthorizedHttpResult, ForbidHttpResult, ProblemDetails>>
{
  private readonly AppDbContext _dbContext;
  private readonly IBlobService _blobService;

  public CreateProvinceEndpoint(AppDbContext dbContext, IBlobService blobService)
  {
    _dbContext = dbContext;
    _blobService = blobService;
  }

  public override void Configure()
  {
    Post("/provinces");
    Summary(s =>
    {
      s.Summary = "Create province";
      s.Description = "Creates a new province.";
    });
    AllowFormData();
    Roles("SystemAdmin");
  }

  public override async Task<Results<Created<ProvinceResponse>, Conflict, UnauthorizedHttpResult, ForbidHttpResult, ProblemDetails>> ExecuteAsync(CreateProvinceRequest req, CancellationToken ct)
  {
      var existingProvince = await _dbContext.Provinces.FirstOrDefaultAsync(x => x.Name.ToLower().Equals(req.Name.ToLower()), ct);
      if (existingProvince != null)
      {
        return TypedResults.Conflict();
      }


      var mapper = new ProvinceMapper();
      var province = mapper.ToEntity(req);

      // Agrega la nueva provincia a la base de datos
      _dbContext.Provinces.Add(province);
      await _dbContext.SaveChangesAsync(ct);


      var response = mapper.FromEntity(province);


      return TypedResults.Created($"/provinces/{province.Id}", response);
  }
}

