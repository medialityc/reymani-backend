using Tests.Mock;

namespace Tests.Tests.Users;

public class SearchUserTests: IClassFixture<CustomWebApplicationFactory>
{
  private readonly CustomWebApplicationFactory _factory;

  public SearchUserTests(CustomWebApplicationFactory factory)
  {
    _factory = factory;
  }

  [Fact]
  public async Task TestOk()
  {
    Assert.True(true);
  }
}