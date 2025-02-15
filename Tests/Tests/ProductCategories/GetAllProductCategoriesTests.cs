using Tests.Mock;

namespace Tests.Tests.ProductCategories;

public class GetAllProductCategoriesTests: IClassFixture<CustomWebApplicationFactory>
{
  private readonly CustomWebApplicationFactory _factory;

  public GetAllProductCategoriesTests(CustomWebApplicationFactory factory)
  {
    _factory = factory;
  }

  [Fact]
  public async Task TestOk()
  {
    Assert.True(true);
  }
}