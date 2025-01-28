using System;
using reymani_web_api.Application.DTOs;

namespace reymani_web_api.Api.Endpoints.Usuario;

public class UpdateUsuarioRequest
{
  public Guid IdUsuario { get; set; }

  public required UsuarioDto Usuario { get; set; }
}

public class UpdateUsuarioRequestValidator : Validator<UpdateUsuarioRequest>
{
  public UpdateUsuarioRequestValidator()
  {
    RuleFor(x => x.Usuario.Id).NotEmpty().WithMessage("El ID del Usuario es requerido")
      .Must((request, idRol) => idRol == request.IdUsuario).WithMessage("El ID del Usuario no coincide con el ID del Usuario a actualizar");

    RuleFor(x => x.Usuario).SetValidator(new UsuarioDtoValidator());
  }
}