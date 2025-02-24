using FastEndpoints;

using FluentValidation;

using reymani_web_api.Endpoints.Provinces.Requests;

namespace reymani_web_api.Endpoints.AddressUser.Requests.Validator
{
  public class UpdateCustomerRequestValidator : Validator<UpdateCustomerRequest>
  {
    public UpdateCustomerRequestValidator()
    {
      RuleFor(x => x.Name).NotEmpty();
      RuleFor(x => x.Address).NotEmpty();
      RuleFor(x => x.IsActive).NotEmpty();

      RuleFor(e => e.Id)
      .NotEmpty()
      .GreaterThan(0);
    }
  }
}
