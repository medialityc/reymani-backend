using Tests.Mock;

namespace Tests.Tests.ProductCategories;

public class DeleteProductCategoryTests: IClassFixture<CustomWebApplicationFactory>
{
  private readonly CustomWebApplicationFactory _factory;

  public DeleteProductCategoryTests(CustomWebApplicationFactory factory)
  {
    _factory = factory;
  }

  [Fact]
  public async Task TestOk()
  {
    Assert.True(true);
  }
}