using System;
using reymani_web_api.Application.DTOs;

namespace reymani_web_api.Api.Endpoints.Negocio;

public class UpdateNegocioRequest
{
  public Guid IdNegocio { get; set; }

  public required NegocioDto Negocio { get; set; }
}

public class UpdateNegocioRequestValidator : Validator<UpdateNegocioRequest>
{
  public UpdateNegocioRequestValidator()
  {
    RuleFor(x => x.Negocio.IdNegocio).NotEmpty().WithMessage("El ID del negocio es requerido")
      .Must((request, idNegocio) => idNegocio == request.IdNegocio).WithMessage("El ID del negocio no coincide con el ID del negocio a actualizar");

    RuleFor(x => x.Negocio).SetValidator(new NegocioDtoValidator());
  }
}
