using System;
using FluentValidation;
using reymani_web_api.Application.DTOs;

namespace reymani_web_api.Api.Endpoints.Rol;

public class UpdateRolRequest
{
  public Guid RolId { get; set; }

  public required RolDto Rol { get; set; }
}

public class UpdateRolRequestValidator : Validator<UpdateRolRequest>
{
  public UpdateRolRequestValidator()
  {
    RuleFor(x => x.Rol.Id).NotEmpty().WithMessage("El ID del rol es requerido")
      .Must((request, idRol) => idRol == request.RolId).WithMessage("El ID del rol no coincide con el ID del rol a actualizar");

    RuleFor(x => x.Rol).SetValidator(new RolDtoValidator());
  }
}

