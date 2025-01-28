namespace reymani_web_api.Api.Endpoints.NegocioUsuario
{
  public class CreateNegocioUsuarioRequest
  {
    public Guid IdUsuario { get; set; }
    public Guid IdNegocio { get; set; }
  }

  public class CreateNegocioUsuarioRequestValidator : Validator<CreateNegocioUsuarioRequest>
  {
    public CreateNegocioUsuarioRequestValidator()
    {
      RuleFor(x => x.IdUsuario).NotEmpty().WithMessage("El campo IdUsuario es obligatorio.");
      RuleFor(x => x.IdNegocio).NotEmpty().WithMessage("El campo IdNegocio es obligatorio.");
    }
  }
}
