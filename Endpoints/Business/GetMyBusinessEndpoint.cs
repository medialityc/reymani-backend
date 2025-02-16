using FastEndpoints;

using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

using reymani_web_api.Data;
using reymani_web_api.Endpoints.Business.Responses;
using reymani_web_api.Endpoints.Mappers;
using reymani_web_api.Services.BlobServices;

namespace reymani_web_api.Endpoints.Business
{
  public class GetMyBusinessEndpoint : EndpointWithoutRequest<Results<Ok<BusinessResponse>, UnauthorizedHttpResult, NotFound, ProblemDetails>>
  {
    private readonly AppDbContext _dbContext;
    private readonly IBlobService _blobService;

    public GetMyBusinessEndpoint(AppDbContext dbContext, IBlobService blobService)
    {
      _dbContext = dbContext;
      _blobService = blobService;
    }

    public override void Configure()
    {
      Get("/business/mine");
      Summary(s =>
      {
        s.Summary = "Get my business";
        s.Description = "Retrieves the business associated with the current user.";
      });
      Roles("BusinessAdmin");
    }

    public override async Task<Results<Ok<BusinessResponse>, UnauthorizedHttpResult, NotFound, ProblemDetails>> ExecuteAsync(CancellationToken ct)
    {
      // Obtener id del token JWT
      var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "Id");
      if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
      {
        return TypedResults.Unauthorized();
      }

      // Buscar el negocio asociado al usuario y activo
      var business = await _dbContext.Businesses
        .Where(b => b.UserId == userId && b.IsActive)
        .Include(b => b.Municipality)
        .ThenInclude(m => m!.Province)
        .FirstOrDefaultAsync(ct);

      if (business is null)
        return TypedResults.NotFound();

      var mapper = new BusinessMapper();
      var response = mapper.ToBusinessResponse(business);

      if (!string.IsNullOrEmpty(business.Logo))
        response.Logo = await _blobService.PresignedGetUrl(business.Logo, ct);

      if (!string.IsNullOrEmpty(business.Banner))
        response.Banner = await _blobService.PresignedGetUrl(business.Banner, ct);

      return TypedResults.Ok(response);
    }
  }
}
