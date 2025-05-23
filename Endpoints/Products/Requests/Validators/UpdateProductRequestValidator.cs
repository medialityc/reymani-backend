using System;
using System.ComponentModel.DataAnnotations;

using FastEndpoints;

using FluentValidation;

using reymani_web_api.Utils.Validations;

namespace reymani_web_api.Endpoints.Products.Requests.Validators;

public class UpdateProductRequestValidator : Validator<UpdateProductRequest>
{
  public UpdateProductRequestValidator()
  {
    RuleFor(x => x.Id).NotEmpty().GreaterThan(0);
    RuleFor(x => x.Name).NotEmpty();
    RuleFor(x => x.IsAvailable).NotNull();
    RuleFor(x => x.IsActive).NotNull();
    RuleFor(x => x.Price).NotEmpty().GreaterThan(0);
    RuleFor(x => x.CategoryId).NotEmpty().GreaterThan(0);
    RuleFor(x => x.Capacity).NotEmpty();

    RuleFor(x => x.DiscountPrice)
        .LessThan(x => x.Price)
        .When(x => x.DiscountPrice.HasValue);

    RuleForEach(x => x.Images)
        .Must(file => file == null || (ImageValidations.BeAValidImage(file) && ImageValidations.HaveValidLength(file)))
        .WithMessage("Cada imagen debe ser válida.");
  }
}