using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

using reymani_web_api.Data;
using reymani_web_api.Endpoints.Mappers;
using reymani_web_api.Endpoints.ShoppingCartItems.Requests;
using reymani_web_api.Endpoints.ShoppingCartItems.Responses;
using reymani_web_api.Services.BlobServices;

namespace reymani_web_api.Endpoints.ShoppingCartItems;

public class GetByIdShoppingCartItemEndpoint : Endpoint<GetByIdShoppingCartItemRequest, Results<Ok<ShoppingCartItemResponse>, NotFound, ProblemDetails>>
{
  private readonly AppDbContext _dbContext;
  private readonly IBlobService _blobService;


  public GetByIdShoppingCartItemEndpoint(AppDbContext dbContext, IBlobService blobService)
  {
    _dbContext = dbContext;
    _blobService = blobService;
  }

  public override void Configure()
  {
    Get("/shoppingcartitem/{id}");
    Summary(s =>
    {
      s.Summary = "Get shopping cart item by Id and Id shopping cart";
      s.Description = "Retrieves details of a shopping cart item by their ID.";
    });
    Roles("Customer");
  }

  public override async Task<Results<Ok<ShoppingCartItemResponse>, NotFound, ProblemDetails>> ExecuteAsync(GetByIdShoppingCartItemRequest req, CancellationToken ct)
  {
    var existingShoppingCart = await _dbContext.ShoppingCarts.FirstOrDefaultAsync(x => x.Id == req.ShoppingCartId, ct);

    if (existingShoppingCart == null)
      return TypedResults.NotFound();


    var item = await _dbContext.ShoppingCartItems
        .Include(p => p.Product)
        .AsNoTracking()
        .FirstOrDefaultAsync(p => p.Id == req.Id && p.ShoppingCartId == req.ShoppingCartId, ct);

    if (item is null)
      return TypedResults.NotFound();

    var mapper = new ShoppingCartItemMapper();
    var _mapperProduct = new ProductMapper();
    var response = mapper.FromEntity(item);

    //Mapeo del producto
    var product = item.Product;

    // Obtener valoraciones del producto
    var ratings = await _dbContext.ProductRatings
      .Where(r => r.ProductId == req.Id)
      .ToListAsync(ct);
    var totalRatings = ratings.Count;
    var averageRating = totalRatings > 0 ? ratings.Average(r => (int)r.Rating) : 0;

    var mapperProduct = new ProductMapper();

    var responseImages = new List<string>();
    if (product!.Images is not null && product.Images.Any())
    {
      foreach (var img in product.Images)
      {
        responseImages.Add(await _blobService.PresignedGetUrl(img, ct));
      }
    }

    response.Product = mapperProduct.ToResponse(product, product.Business!.Name, product.Category!.Name, responseImages, totalRatings, (decimal)averageRating);

    return TypedResults.Ok(response);
  }
}
