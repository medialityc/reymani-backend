using FastEndpoints;

using FluentValidation;

namespace reymani_web_api.Endpoints.ShoppingCarts.Requests.Validators;

public class SearchItemInShoppingCartRequestValidator : Validator<SearchItemInShoppingCartRequest>
{
  public SearchItemInShoppingCartRequestValidator()
  {
    RuleFor(x => x.Page)
        .GreaterThan(0);

    RuleFor(x => x.PageSize)
        .GreaterThan(0);
  }
}
