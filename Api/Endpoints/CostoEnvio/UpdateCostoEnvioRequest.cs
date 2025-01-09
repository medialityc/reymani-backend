using System;
using FluentValidation;
using reymani_web_api.Application.DTOs;

namespace reymani_web_api.Api.Endpoints.CostoEnvio;

public class UpdateCostoEnvioRequest
{
  public Guid CostoEnvioId { get; set; }
  public required CostoEnvioDto CostoEnvio { get; set; }
}

public class UpdateCostoEnvioRequestValidator : Validator<UpdateCostoEnvioRequest>
{
  public UpdateCostoEnvioRequestValidator()
  {
    RuleFor(x => x.CostoEnvio.IdCostoEnvio).NotEmpty().WithMessage("El ID del costo de envío es requerido")
      .Must((request, idCostoEnvio) => idCostoEnvio == request.CostoEnvioId).WithMessage("El ID del costo de envío no coincide con el ID del costo de envío a actualizar");

    RuleFor(x => x.CostoEnvio).SetValidator(new CostoEnvioDtoValidator());
  }
}
