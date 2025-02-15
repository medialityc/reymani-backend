using Tests.Mock;

namespace Tests.Tests.ProductCategories;

public class UpdateProductCategoryTests: IClassFixture<CustomWebApplicationFactory>
{
  private readonly CustomWebApplicationFactory _factory;

  public UpdateProductCategoryTests(CustomWebApplicationFactory factory)
  {
    _factory = factory;
  }

  [Fact]
  public async Task TestOk()
  {
    Assert.True(true);
  }
}