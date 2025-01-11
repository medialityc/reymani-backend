namespace reymani_web_api.Api.Endpoints.NegocioCliente
{
  public class CreateNegocioClienteRequest
  {
    public Guid IdCliente { get; set; }
    public Guid IdNegocio { get; set; }
  }

  public class CreateNegocioClienteRequestValidator : Validator<CreateNegocioClienteRequest>
  {
    public CreateNegocioClienteRequestValidator()
    {
      RuleFor(x => x.IdCliente).NotEmpty().WithMessage("El campo IdCliente es obligatorio.");
      RuleFor(x => x.IdNegocio).NotEmpty().WithMessage("El campo IdNegocio es obligatorio.");
    }
  }
}
