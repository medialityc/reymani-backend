using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using reymani_web_api.Data;
using reymani_web_api.Data.Models;
using reymani_web_api.Endpoints.Products.Requests;

namespace reymani_web_api.Endpoints.Products
{
  public class RateProductEndpoint : Endpoint<RateProductRequest, Results<Ok, UnauthorizedHttpResult, ProblemDetails, NotFound>>
  {
    private readonly AppDbContext _dbContext;
    public RateProductEndpoint(AppDbContext dbContext)
    {
      _dbContext = dbContext;
    }

    public override void Configure()
    {
      Post("/products/rate");
      Summary(s =>
      {
        s.Summary = "Rate or update rating for a product";
        s.Description = "Creates or updates a rating for the specified product for the logged in user.";
      });
      Roles("Customer");
    }

    public override async Task<Results<Ok, UnauthorizedHttpResult, ProblemDetails, NotFound>> ExecuteAsync(RateProductRequest req, CancellationToken ct)
    {
      var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "Id");
      if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
        return TypedResults.Unauthorized();

      // Obtener el usuario
      var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == userId, ct);
      if(user == null)
        return TypedResults.Unauthorized();

      // Obtener el producto con su negocio incluido
      var product = await _dbContext.Products
        .Include(p => p.Business)
        .FirstOrDefaultAsync(p => p.Id == req.ProductId, ct);
      if(product == null)
        return TypedResults.NotFound();

      // Verificar si ya existe una valoración para este usuario y producto
      var existingRating = await _dbContext.ProductRatings
        .FirstOrDefaultAsync(r => r.ProductId == req.ProductId && r.UserId == userId, ct);

      if (existingRating != null)
      {
        existingRating.Rating = req.Rating;
        if(existingRating.User == null)
          existingRating.User = user;
        if(existingRating.Product == null)
          existingRating.Product = product;
      }
      else
      {
        var newRating = new ProductRating
        {
          ProductId = req.ProductId,
          UserId = userId,
          Rating = req.Rating,
          User = user,
          Product = product
        };
        _dbContext.ProductRatings.Add(newRating);
      }

      await _dbContext.SaveChangesAsync(ct);
      return TypedResults.Ok();
    }
  }
}
