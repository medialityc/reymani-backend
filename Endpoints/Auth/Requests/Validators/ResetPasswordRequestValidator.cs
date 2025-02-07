using FluentValidation;

using reymani_web_api.Endpoints.Auth.Requests;

namespace reymani_web_api.Endpoints.Auth.Requests.Validators
{
  public class ResetPasswordRequestValidator : AbstractValidator<ResetPasswordRequest>
  {
    public ResetPasswordRequestValidator()
    {
      // Validar que el código de confirmación tenga 4 dígitos numéricos
      RuleFor(x => x.ConfirmationCode)
        .NotEmpty().WithMessage("Confirmation code is required.")
        .Matches(@"^\d{4}$").WithMessage("Confirmation code must be 4 digits.");


      RuleFor(x => x.Password)
        .NotEmpty()
        .MinimumLength(8)
        .Matches(@"[A-Z]").WithMessage("Password must contain at least one uppercase letter.")
        .Matches(@"[0-9]").WithMessage("Password must contain at least one number.")
        .Matches(@"[\W]").WithMessage("Password must contain at least one special character.");
    }
  }
}
