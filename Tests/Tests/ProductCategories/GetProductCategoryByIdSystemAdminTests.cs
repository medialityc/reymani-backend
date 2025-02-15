using Tests.Mock;

namespace Tests.Tests.ProductCategories;

public class GetProductCategoryByIdSystemAdminTests: IClassFixture<CustomWebApplicationFactory>
{
  private readonly CustomWebApplicationFactory _factory;

  public GetProductCategoryByIdSystemAdminTests(CustomWebApplicationFactory factory)
  {
    _factory = factory;
  }

  [Fact]
  public async Task TestOk()
  {
    Assert.True(true);
  }
}