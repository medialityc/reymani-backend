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

    // Mapea las propiedades del UpdateProductRequest a la entidad Product.
    public Product UpdateEntity(UpdateProductRequest req, Product product)
    {
      product.Name = req.Name;
      product.Description = req.Description;
      product.IsAvailable = req.IsAvailable;
      product.IsActive = req.IsActive;
      product.Price = req.Price;
      product.DiscountPrice = req.DiscountPrice;
      product.CategoryId = req.CategoryId;
      product.Capacity = req.Capacity;
      return product;
    }

    // Mapea la entidad Product a la respuesta, incluyendo nombres de negocio y categoría, y las imagenes presignadas.
    public ProductResponse ToResponse(Product product, string businessName, string categoryName, List<string> responseImages, int numberOfRatings, decimal averageRating)
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
        Capacity = (int)product.Capacity,
        Images = responseImages,
        NumberOfRatings = numberOfRatings,
        AverageRating = averageRating
      };
    }

    // Mapea la entidad Product a una respuesta simple.
    public SimpleProductResponse ToSimpleProductResponse(Product product, List<string> images, int numberOfRatings, decimal averageRating)
    {
      return new SimpleProductResponse
      {
        Id = product.Id,
        Name = product.Name,
        Description = product.Description,
        IsAvailable = product.IsAvailable,
        Images = images,
        Price = product.Price,
        DiscountPrice = product.DiscountPrice,
        Capacity = (int)product.Capacity,
        NumberOfRatings = numberOfRatings,
        AverageRating = averageRating
      };
    }
  }
}
