using FastEndpoints;

using FluentValidation;

namespace reymani_web_api.Endpoints.AddressUser.Requests.Validator;

public class CreateCustomerRequestValidator : Validator<CreateCustomerRequest>
{
  public CreateCustomerRequestValidator()
  {
    RuleFor(x => x.Name).NotEmpty();
    RuleFor(x => x.Address).NotEmpty();
  }
}
