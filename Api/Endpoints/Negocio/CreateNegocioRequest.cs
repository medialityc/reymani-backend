using System.Data;

namespace reymani_web_api.Api.Endpoints.Negocio;

public class CreateNegocioRequest
{
  public required string Nombre { get; set; }
  public required string Descripcion { get; set; }
  public bool EntregaDomicilio { get; set; }
  public string? URLImagenPrincipal { get; set; }
  public string? URLImagenLogo { get; set; }
  public string? URLImagenBanner { get; set; }

}

public class CreateNegocioRequestValidator : Validator<CreateNegocioRequest>
{
  public CreateNegocioRequestValidator()
  {
    RuleFor(x => x.Nombre)
      .NotEmpty().WithMessage("El nombre es obligatorio.")
      .MaximumLength(100).WithMessage("El nombre no puede tener más de 100 caracteres.");

    RuleFor(x => x.Descripcion)
      .NotEmpty().WithMessage("La descripción es obligatoria.")
      .MaximumLength(500).WithMessage("La descripción no puede tener más de 500 caracteres.");

    RuleFor(x => x.EntregaDomicilio)
      .NotNull().WithMessage("El campo entrega a domicilio es obligatorio.");

    RuleFor(x => x.URLImagenPrincipal)
      .Must(uri => Uri.IsWellFormedUriString(uri, UriKind.Absolute)).WithMessage("La URL de la imagen principal no es válida.")
      .When(x => !string.IsNullOrEmpty(x.URLImagenPrincipal));

    RuleFor(x => x.URLImagenLogo)
      .Must(uri => Uri.IsWellFormedUriString(uri, UriKind.Absolute)).WithMessage("La URL del logo no es válida.")
      .When(x => !string.IsNullOrEmpty(x.URLImagenLogo));

    RuleFor(x => x.URLImagenBanner)
      .Must(uri => Uri.IsWellFormedUriString(uri, UriKind.Absolute)).WithMessage("La URL del banner no es válida.")
      .When(x => !string.IsNullOrEmpty(x.URLImagenBanner));
  }
}
