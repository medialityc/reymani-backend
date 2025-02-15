using Tests.Mock;

namespace Tests.Tests.Users;

public class UpdateUsersMeTests: IClassFixture<CustomWebApplicationFactory>
{
  private readonly CustomWebApplicationFactory _factory;

  public UpdateUsersMeTests(CustomWebApplicationFactory factory)
  {
    _factory = factory;
  }

  [Fact]
  public async Task TestOk()
  {
    Assert.True(true);
  }
}