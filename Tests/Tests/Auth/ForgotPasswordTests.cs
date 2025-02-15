using Tests.Mock;

namespace Tests.Tests.Auth;

public class ForgotPasswordTests: IClassFixture<CustomWebApplicationFactory>
{
  private readonly CustomWebApplicationFactory _factory;

  public ForgotPasswordTests(CustomWebApplicationFactory factory)
  {
    _factory = factory;
  }

  [Fact]
  public async Task TestOk()
  {
    Assert.True(true);
  }
}