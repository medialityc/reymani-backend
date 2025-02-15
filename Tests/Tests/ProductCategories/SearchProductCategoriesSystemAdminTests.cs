using Tests.Mock;

namespace Tests.Tests.ProductCategories;

public class SearchProductCategoriesSystemAdminTests: IClassFixture<CustomWebApplicationFactory>
{
  private readonly CustomWebApplicationFactory _factory;

  public SearchProductCategoriesSystemAdminTests(CustomWebApplicationFactory factory)
  {
    _factory = factory;
  }

  [Fact]
  public async Task TestOk()
  {
    Assert.True(true);
  }
}