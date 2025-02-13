using System;

using reymani_web_api.Endpoints.ProductCategories.Requests;
using reymani_web_api.Endpoints.ProductCategories.Responses;

using ReymaniWebApi.Data.Models;

namespace reymani_web_api.Utils.Mappers;

public class ProductCategoryMapper
{
  // Convierte un CreateProductCategoryRequest en una entidad ProductCategory
  public ProductCategory ToEntity(CreateProductCategoryRequest r) => new()
  {
    Name = r.Name,
    Logo = string.Empty,
    IsActive = true
  };

  // Actualiza una entidad ProductCategory existente a partir de un UpdateProductCategoryRequest
  public ProductCategory ToEntity(UpdateProductCategoryRequest r, ProductCategory existing)
  {
    existing.Name = r.Name;
    existing.Logo = string.Empty;
    existing.IsActive = r.IsActive;
    return existing;
  }

  // Convierte una entidad ProductCategory en un ProductCategoryResponse
  public ProductCategoryResponse FromEntity(ProductCategory e) => new()
  {
    Id = e.Id,
    Name = e.Name,
    Logo = string.Empty,
    IsActive = e.IsActive
  };
}
