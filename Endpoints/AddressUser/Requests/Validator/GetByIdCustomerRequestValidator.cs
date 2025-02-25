using FastEndpoints;
using FluentValidation;

using reymani_web_api.Endpoints.Provinces.Requests;

namespace reymani_web_api.Endpoints.AddressUser.Requests.Validator;

public class GetByIdCustomerRequestValidator : Validator<GetByIdCustomerRequest>
{
  public GetByIdCustomerRequestValidator()
  {
    RuleFor(e => e.Id)
      .NotEmpty()
      .GreaterThan(0);
  }
}
