using Tests.Mock;

namespace Tests.Tests.Users;

public class GetUserMeTests: IClassFixture<CustomWebApplicationFactory>
{
  private readonly CustomWebApplicationFactory _factory;

  public GetUserMeTests(CustomWebApplicationFactory factory)
  {
    _factory = factory;
  }

  [Fact]
  public async Task TestOk()
  {
    Assert.True(true);
  }
}