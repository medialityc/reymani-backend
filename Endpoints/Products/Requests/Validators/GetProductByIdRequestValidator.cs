using System;

using FastEndpoints;

using FluentValidation;

namespace reymani_web_api.Endpoints.Products.Requests.Validators;

public class GetProductByIdRequestValidator : Validator<GetProductByIdRequest>
{
  public GetProductByIdRequestValidator()
  {
    RuleFor(x => x.Id).GreaterThan(0);
  }
}