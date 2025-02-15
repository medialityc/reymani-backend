using Tests.Mock;

namespace Tests.Tests.ProductCategories;

public class GetAllProductCategoriesSystemAdminTests: IClassFixture<CustomWebApplicationFactory>
{
  private readonly CustomWebApplicationFactory _factory;

  public GetAllProductCategoriesSystemAdminTests(CustomWebApplicationFactory factory)
  {
    _factory = factory;
  }

  [Fact]
  public async Task TestOk()
  {
    Assert.True(true);
  }
}