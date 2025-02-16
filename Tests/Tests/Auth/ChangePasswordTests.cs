using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

using FluentAssertions;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

using reymani_web_api.Data;
using reymani_web_api.Endpoints.Auth.Requests;
using reymani_web_api.Endpoints.Auth.Responses;

using Tests.Mock;
using Tests.Utils;

namespace Tests.Tests.Auth;

public class ChangePasswordTests(CustomWebApplicationFactory factory) : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client = factory.CreateClient();

    [Fact]
    public async Task TestOk_ChangePasswordSuccessfully()
    {
        var loginRequest = new LoginRequest
        {
            Email = "active_confirmed@example.com",
            Password = "Password123!"
        };

        var loginResponse = await _client.PostAsJsonAsync("/auth/login", loginRequest);
        var loginContent = await loginResponse.Content.ReadAsStringAsync();
        var loginResult = JsonSerializer.Deserialize<LoginResponse>(loginContent, JsonSerializerOp.GetOptions());

        _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", loginResult!.Token);

        var changePasswordRequest = new ChangePasswordRequest
        {
            CurrentPassword = "Password123!",
            NewPassword = "Newpassword123!"
        };
        
        var response = await _client.PostAsJsonAsync("/auth/change-password", changePasswordRequest);
        var content = await response.Content.ReadAsStringAsync();
        
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        content.Should().Be("\"Password changed successfully\"");
        
        using var scope = factory.Services.CreateScope();
        // var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        // var user = await dbContext.Users.FirstOrDefaultAsync(u => u.Email == loginRequest.Email);
        // user.Should().NotBeNull();
        // user.Password.Should().NotBeNullOrEmpty();
        // BCrypt.Net.BCrypt.Verify("Newpassword123!", user.Password).Should().BeTrue();
        
        changePasswordRequest.NewPassword = "Password123!";
        changePasswordRequest.CurrentPassword = "Newpassword123!";
        await _client.PostAsJsonAsync("/auth/change-password", changePasswordRequest);
    }

    [Fact]
    public async Task TestUnauthorized_InvalidCurrentPassword()
    {
        var loginRequest = new LoginRequest
        {
            Email = "active_confirmed@example.com",
            Password = "Password123!"
        };

        var loginResponse = await _client.PostAsJsonAsync("/auth/login", loginRequest);
        var loginContent = await loginResponse.Content.ReadAsStringAsync();
        var loginResult = JsonSerializer.Deserialize<LoginResponse>(loginContent, JsonSerializerOp.GetOptions());

        _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", loginResult!.Token);

        var changePasswordRequest = new ChangePasswordRequest
        {
            CurrentPassword = "wrongpassword",
            NewPassword = "Password1234!"
        };
        
        var response = await _client.PostAsJsonAsync("/auth/change-password", changePasswordRequest);

        var content = await response.Content.ReadAsStringAsync();
        
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        content.Should().Contain("Invalid password");
    }

    [Fact]
    public async Task TestUnauthorized_UserNotAuthenticated()
    {
        var changePasswordRequest = new ChangePasswordRequest
        {
            CurrentPassword = "password123",
            NewPassword = "newpassword123"
        };
        
        var response = await _client.PostAsJsonAsync("/auth/change-password", changePasswordRequest);
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
}
