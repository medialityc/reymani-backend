using System;

using FastEndpoints;

using FluentValidation;

namespace reymani_web_api.Endpoints.Business.Requests.Validators;
public class GetBusinessByIdRequestValidator : Validator<GetBusinessByIdRequest>
{
  public GetBusinessByIdRequestValidator()
  {
    RuleFor(e => e.Id)
      .NotEmpty()
      .GreaterThan(0);
  }
}
