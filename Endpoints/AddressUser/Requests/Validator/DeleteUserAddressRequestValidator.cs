using FastEndpoints;

using FluentValidation;

using reymani_web_api.Endpoints.Provinces.Requests;

namespace reymani_web_api.Endpoints.AddressUser.Requests.Validator;

public class DeleteUserAddressRequestValidator : Validator<DeleteUserAddressRequest>
{
  public DeleteUserAddressRequestValidator()
  {
    RuleFor(e => e.Id)
      .NotEmpty()
      .GreaterThan(0);
  }
}
