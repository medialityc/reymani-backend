using Tests.Mock;

namespace Tests.Tests.ProductCategories;

public class GetByIdSystemAdminTests: IClassFixture<CustomWebApplicationFactory>
{
  private readonly CustomWebApplicationFactory _factory;

  public GetByIdSystemAdminTests(CustomWebApplicationFactory factory)
  {
    _factory = factory;
  }

  [Fact]
  public async Task TestOk()
  {
    Assert.True(true);
  }
}