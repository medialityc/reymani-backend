using FastEndpoints;

using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

using reymani_web_api.Data;
using reymani_web_api.Endpoints.Business.Requests;

namespace reymani_web_api.Endpoints.Business
{
  public class DeleteBusinessEndpoint : Endpoint<GetBusinessByIdRequest, Results<Ok, NotFound, Conflict, ProblemDetails>>
  {
    private readonly AppDbContext dbContext;

    public DeleteBusinessEndpoint(AppDbContext dbContext)
    {
      this.dbContext = dbContext;
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
      var business = await dbContext.Businesses
        .Include(b => b.Products)
        .FirstOrDefaultAsync(b => b.Id == req.Id, ct);

      if (business is null)
        return TypedResults.NotFound();

      if (business.Products?.Any() == true)
        return TypedResults.Conflict();

      dbContext.Businesses.Remove(business);
      await dbContext.SaveChangesAsync(ct);

      return TypedResults.Ok();
    }
  }
}
