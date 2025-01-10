using System;
using FluentValidation;
using reymani_web_api.Application.DTOs;

namespace reymani_web_api.Api.Endpoints.HorarioNegocio;

public class UpdateHorarioNegocioRequest
{
  public Guid IdHorario { get; set; }
  public required HorarioNegocioDto HorarioNegocio { get; set; }
}

public class UpdateHorarioNegocioRequestValidator : Validator<UpdateHorarioNegocioRequest>
{
  public UpdateHorarioNegocioRequestValidator()
  {
    RuleFor(x => x.HorarioNegocio.IdHorario).NotEmpty().WithMessage("El ID del horario de negocio es requerido")
      .Must((request, idHorario) => idHorario == request.IdHorario).WithMessage("El ID del horario de negocio no coincide con el ID del horario de negocio a actualizar");

    RuleFor(x => x.HorarioNegocio).SetValidator(new HorarioNegocioDtoValidator());
  }
}
