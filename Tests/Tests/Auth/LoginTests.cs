using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

using FluentAssertions;

using reymani_web_api.Endpoints.Auth.Requests;
using reymani_web_api.Endpoints.Auth.Responses;

using Tests.Mock;
using Tests.Utils;

namespace Tests.Tests.Auth;

public class LoginTests(CustomWebApplicationFactory factory) : IClassFixture<CustomWebApplicationFactory>
{
  private readonly HttpClient _client = factory.CreateClient();

  [Fact]
    public async Task TestOk_ActiveAndConfirmedUser()
    {
        var loginRequest = new LoginRequest
        {
            Email = "active_confirmed@example.com",
            Password = "Password123!"
        };
        
        var response = await _client.PostAsJsonAsync("/auth/login", loginRequest);
        
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var content = await response.Content.ReadAsStringAsync();
        var loginResponse = JsonSerializer.Deserialize<LoginResponse>(content, JsonSerializerOp.GetOptions());

        loginResponse.Should().NotBeNull();
        loginResponse.Token.Should().NotBeEmpty();
    }

    [Fact]
    public async Task TestUnauthorized_InactiveUser()
    {
        var loginRequest = new LoginRequest
        {
            Email = "inactive@example.com",
            Password = "password123"
        };
        
        var response = await _client.PostAsJsonAsync("/auth/login", loginRequest);
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task TestUnauthorized_UnconfirmedUser()
    {
        var loginRequest = new LoginRequest
        {
            Email = "unconfirmed@example.com",
            Password = "password123"
        };
        
        var response = await _client.PostAsJsonAsync("/auth/login", loginRequest);
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task TestUnauthorized_BadCredentials()
    {
        var loginRequest = new LoginRequest
        {
            Email = "active_confirmed@example.com",
            Password = "wrongpassword"
        };
        var response = await _client.PostAsJsonAsync("/auth/login", loginRequest);
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
}