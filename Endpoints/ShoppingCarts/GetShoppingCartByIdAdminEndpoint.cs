using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

using reymani_web_api.Data;
using reymani_web_api.Endpoints.Mappers;
using reymani_web_api.Endpoints.ShoppingCarts.Requests;
using reymani_web_api.Endpoints.ShoppingCarts.Responses;
using reymani_web_api.Services.BlobServices;
using ReymaniWebApi.Data.Models;

namespace reymani_web_api.Endpoints.ShoppingCarts;

public class GetShoppingCartByIdAdminEndpoint : Endpoint<GetShoppingCartByIdRequest,Results<Ok<ShoppingCartResponse>, NotFound, UnauthorizedHttpResult, ProblemDetails>>
{
  private readonly AppDbContext _dbContext;
  private readonly IBlobService _blobService;


  public GetShoppingCartByIdAdminEndpoint(AppDbContext dbContext, IBlobService blobService)
  {
    _dbContext = dbContext;
    _blobService = blobService;
  }

  public override void Configure()
  {
    Get("/shoppingcart/admin/{id}");
    Summary(s =>
    {
      s.Summary = "Get shopping cart by Id";
      s.Description = "Retrieves details of a shopping cart item by their ID.";
    });
    Roles("SystemAdmin");
  }

  public override async Task<Results<Ok<ShoppingCartResponse>, NotFound, UnauthorizedHttpResult, ProblemDetails>> ExecuteAsync(GetShoppingCartByIdRequest req, CancellationToken ct)
  {
    var existingShoppingCart = await _dbContext.ShoppingCarts.FirstOrDefaultAsync(x => x.Id == req.ShoppingCartId, ct);

    if (existingShoppingCart == null)
      return TypedResults.NotFound();


    var shoppingCart = await _dbContext.ShoppingCarts
        .Include(p => p.Items)
        .AsNoTracking()
        .FirstOrDefaultAsync(p => p.Id == req.ShoppingCartId, ct);

    if (shoppingCart is null)
      return TypedResults.NotFound();

    var mapper = new ShoppingCartMapper();
    var _mapperProduct = new ProductMapper();
    var _mapperShoppingCartItem = new ShoppingCartItemMapper();

    var response = mapper.FromEntity(shoppingCart);

    if (shoppingCart.Items != null)
    {
      foreach (var i in shoppingCart.Items)
      {
        var shoppingCartItem = _mapperShoppingCartItem.FromEntity(i);


        //Mapeo del producto
        var product = i.Product;

        // Obtener valoraciones del producto
        var ratings = await _dbContext.ProductRatings
          .Where(r => r.ProductId == req.ShoppingCartId)
          .ToListAsync(ct);
        var totalRatings = ratings.Count;
        var averageRating = totalRatings > 0 ? ratings.Average(r => (int)r.Rating) : 0;

        var mapperProduct = new ProductMapper();

        var responseImages = new List<string>();
        if (product?.Images is not null && product.Images.Any())
        {
          foreach (var img in product.Images)
          {
            responseImages.Add(await _blobService.PresignedGetUrl(img, ct));
          }
        }
        var productResponse = _mapperProduct.ToResponse(product, product.Business!.Name, product.Category!.Name, responseImages, totalRatings, (decimal)averageRating);
        shoppingCartItem.Product = productResponse;
        response?.Items?.Add(shoppingCartItem);
      }
    }
    return TypedResults.Ok(response);
  }
}
