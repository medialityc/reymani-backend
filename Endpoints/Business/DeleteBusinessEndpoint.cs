using FastEndpoints;

using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

using reymani_web_api.Data;
using reymani_web_api.Endpoints.Business.Requests;
using reymani_web_api.Services.BlobServices;

namespace reymani_web_api.Endpoints.Business
{
  public class DeleteBusinessEndpoint : Endpoint<GetBusinessByIdRequest, Results<Ok, NotFound, Conflict, ProblemDetails>>
  {
    private readonly AppDbContext _dbContext;
    private readonly IBlobService _blobService;

    public DeleteBusinessEndpoint(AppDbContext dbContext, IBlobService blobService)
    {
      _dbContext = dbContext;
      _blobService = blobService;
    }

    public override void Configure()
    {
      Delete("/business/{id}");
      Summary(s =>
      {
        s.Summary = "Delete business";
        s.Description = "Deletes a business by ID if it does not have associated products.";
      });
      Roles("SystemAdmin");
    }

    public override async Task<Results<Ok, NotFound, Conflict, ProblemDetails>> ExecuteAsync(GetBusinessByIdRequest req, CancellationToken ct)
    {
      var business = await _dbContext.Businesses
        .Include(b => b.Products)
        .FirstOrDefaultAsync(b => b.Id == req.Id, ct);

      if (business is null)
        return TypedResults.NotFound();

      if (business.Products?.Any() == true)
        return TypedResults.Conflict();

      _dbContext.Businesses.Remove(business);
      await _dbContext.SaveChangesAsync(ct);

      if (!string.IsNullOrEmpty(business.Banner))
        await _blobService.DeleteObject(business.Banner, ct);

      if (!string.IsNullOrEmpty(business.Logo))
        await _blobService.DeleteObject(business.Logo, ct);

      return TypedResults.Ok();
    }
  }
}