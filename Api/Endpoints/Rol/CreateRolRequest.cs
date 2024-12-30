using System;

namespace reymani_web_api.Api.Endpoints.Rol;

public class CreateRolRequest
{
  public required string Nombre { get; set; }
  public string? Descripcion { get; set; }
  public required List<Guid> Permisos { get; set; }
}

public class CreateRolRequestValidator : Validator<CreateRolRequest>
{
  public CreateRolRequestValidator()
  {
    RuleFor(x => x.Nombre).NotEmpty().WithMessage("El nombre es requerido")
      .MaximumLength(50).WithMessage("El nombre no debe exceder los 50 caracteres")
      .Matches("^[a-zA-Z0-9 ]*$").WithMessage("El nombre solo puede contener letras y números");

    RuleFor(x => x.Descripcion).MaximumLength(100).WithMessage("La descripción no debe exceder los 100 caracteres")
      .Matches("^[a-zA-Z0-9 ]*$").WithMessage("La descripción solo puede contener letras y números")
      .When(x => x.Descripcion != null);
  }
}