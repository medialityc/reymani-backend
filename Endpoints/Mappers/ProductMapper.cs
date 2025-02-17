using System;
using reymani_web_api.Endpoints.Products.Requests;
using reymani_web_api.Endpoints.Products.Responses;
using ReymaniWebApi.Data.Models;
using System.Collections.Generic;

namespace reymani_web_api.Endpoints.Mappers
{
  public class ProductMapper
  {
    // Mapea el request a la entidad Product, se requiere el BusinessId obtenido en el endpoint.
    public Product ToEntity(CreateProductRequest req, int businessId)
    {
      return new Product
      {
        Name = req.Name,
        Description = req.Description,
        BusinessId = businessId,
        IsAvailable = req.IsAvailable,
        IsActive = req.IsActive,
        Price = req.Price,
        DiscountPrice = req.DiscountPrice,
        CategoryId = req.CategoryId,
        Capacity = req.Capacity,
        Images = new List<string>() // Inicializa lista vacía para imagenes
      };
    }

    // Mapea de CreateMyProductRequest a la entidad Product, utilizando el BusinessId obtenido en el endpoint.
    public Product ToEntity(CreateMyProductRequest req, int businessId)
    {
      return new Product
      {
        Name = req.Name,
        Description = req.Description,
        BusinessId = businessId,
        IsAvailable = req.IsAvailable,
        IsActive = req.IsActive,
        Price = req.Price,
        DiscountPrice = req.DiscountPrice,
        CategoryId = req.CategoryId,
        Capacity = req.Capacity,
        Images = new List<string>() // Inicializa lista vacía para imágenes
      };
    }

    // Mapea la entidad Product a la respuesta, incluyendo nombres de negocio y categoría, y las imagenes presignadas.
    public ProductResponse ToResponse(Product product, string businessName, string categoryName, List<string> responseImages)
    {
      return new ProductResponse
      {
        Id = product.Id,
        Name = product.Name,
        Description = product.Description,
        BusinessId = product.BusinessId,
        BusinessName = businessName,
        IsAvailable = product.IsAvailable,
        IsActive = product.IsActive,
        Price = product.Price,
        DiscountPrice = product.DiscountPrice,
        CategoryId = product.CategoryId,
        CategoryName = categoryName,
        Capacity = product.Capacity,
        Images = responseImages
      };
    }
  }
}
