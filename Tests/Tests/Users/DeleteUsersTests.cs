using Tests.Mock;

namespace Tests.Tests.Users;

public class DeleteUsersTests: IClassFixture<CustomWebApplicationFactory>
{
  private readonly CustomWebApplicationFactory _factory;

  public DeleteUsersTests(CustomWebApplicationFactory factory)
  {
    _factory = factory;
  }

  [Fact]
  public async Task TestOk()
  {
    Assert.True(true);
  }
}