using Tests.Mock;

namespace Tests.Tests.ProductCategories;

public class GetProductCategoryByIdTests: IClassFixture<CustomWebApplicationFactory>
{
  private readonly CustomWebApplicationFactory _factory;

  public GetProductCategoryByIdTests(CustomWebApplicationFactory factory)
  {
    _factory = factory;
  }

  [Fact]
  public async Task TestOk()
  {
    Assert.True(true);
  }
}