using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

using reymani_web_api.Data;
using reymani_web_api.Endpoints.Auth.Requests;
using Tests.Mock;

namespace Tests.Tests.Auth;

public class ForgotPasswordTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;
    private readonly CustomWebApplicationFactory _factory;

    public ForgotPasswordTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task TestOk_ForgotPasswordSuccessfully()
    {
        // Arrange
        var forgotPasswordRequest = new ForgotPasswordRequest
        {
            Email = "admin@example.com"
        };
        
        var response = await _client.PostAsJsonAsync("/auth/forgot-password", forgotPasswordRequest);
        
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var content = await response.Content.ReadAsStringAsync();
        content.Should().Be("\"A reset code has been sent to your email.\"");
    }

    [Fact]
    public async Task TestOk_UserDoesNotExist()
    {
        var forgotPasswordRequest = new ForgotPasswordRequest
        {
            Email = "nonexistent@example.com"
        };
        var response = await _client.PostAsJsonAsync("/auth/forgot-password", forgotPasswordRequest);
        
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var content = await response.Content.ReadAsStringAsync();
        content.Should().Be("\"A reset code has been sent to your email.\"");
    }
}
