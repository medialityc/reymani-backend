using System;
using FluentValidation;
using reymani_web_api.Application.DTOs;

namespace reymani_web_api.Api.Endpoints.CategoriaNegocio;

public class UpdateCategoriaNegocioRequest
{
  public Guid CategoriaNegocioId { get; set; }
  public required CategoriaNegocioDto CategoriaNegocio { get; set; }
}

public class UpdateCategoriaNegocioRequestValidator : Validator<UpdateCategoriaNegocioRequest>
{
  public UpdateCategoriaNegocioRequestValidator()
  {
    RuleFor(x => x.CategoriaNegocio.IdCategoria).NotEmpty().WithMessage("El ID de la categoría de negocio es requerido")
      .Must((request, idCategoria) => idCategoria == request.CategoriaNegocioId).WithMessage("El ID de la categoría de negocio no coincide con el ID de la categoría de negocio a actualizar");

    RuleFor(x => x.CategoriaNegocio).SetValidator(new CategoriaNegocioDtoValidator());
  }
}
