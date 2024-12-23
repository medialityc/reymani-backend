using System;
using reymani_web_api.Application.DTOs;

namespace reymani_web_api.Api.Endpoints.Cliente;

public class UpdateClienteRequest
{
  public Guid IdCliente { get; set; }

  public required ClienteDto Cliente { get; set; }
}

public class UpdateClienteRequestValidator : Validator<UpdateClienteRequest>
{
  public UpdateClienteRequestValidator()
  {
    RuleFor(x => x.Cliente.Id).NotEmpty().WithMessage("El ID del cliente es requerido")
      .Must((request, idRol) => idRol == request.IdCliente).WithMessage("El ID del cliente no coincide con el ID del cliente a actualizar");

    RuleFor(x => x.Cliente).SetValidator(new ClienteDtoValidator());
  }
}