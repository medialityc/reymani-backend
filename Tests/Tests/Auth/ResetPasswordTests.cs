using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

using reymani_web_api.Data;
using reymani_web_api.Endpoints.Auth.Requests;
using Tests.Mock;

namespace Tests.Tests.Auth;

public class ResetPasswordTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;
    private readonly CustomWebApplicationFactory _factory;

    public ResetPasswordTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
    }

    // [Fact]
    // public async Task TestOk_ResetPasswordSuccessfully()
    // {
    //   using var scope = _factory.Services.CreateScope();
    //   var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    //   dbContext.ForgotPasswordNumbers.Add(new() { UserId = 3, Number = "1234" });
    //   await dbContext.SaveChangesAsync();
    //   
    //     var resetPasswordRequest = new ResetPasswordRequest
    //     {
    //         Email = "business@example.com",
    //         ConfirmationCode = "1234",
    //         Password = "Newpassword123!"
    //     };
    //     
    //     var response = await _client.PostAsJsonAsync("/auth/reset-password", resetPasswordRequest);
    //     
    //     response.StatusCode.Should().Be(HttpStatusCode.OK);
    //
    //     var content = await response.Content.ReadAsStringAsync();
    //     content.Should().Be("\"Password reset successfully.\"");
    //     
    //     var user = await dbContext.Users.FirstOrDefaultAsync(u => u.Email == resetPasswordRequest.Email);
    //     user.Should().NotBeNull();
    //     BCrypt.Net.BCrypt.Verify("Newpassword123!", user.Password).Should().BeTrue();
    //
    //     await dbContext.ForgotPasswordNumbers.AddAsync(new()
    //     {
    //       Number = "1234", UserId = 3
    //     });
    //     await dbContext.SaveChangesAsync();
    // }

    [Fact]
    public async Task TestNotFound_ConfirmationCodeDoesNotExist()
    {
        var resetPasswordRequest = new ResetPasswordRequest
        {
            Email = "business@example.com",
            ConfirmationCode = "9999",
            Password = "Newpassword123!"
        };
        
        var response = await _client.PostAsJsonAsync("/auth/reset-password", resetPasswordRequest);
        
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task TestNotFound_UserDoesNotExist()
    {
        var resetPasswordRequest = new ResetPasswordRequest
        {
            Email = "nonexistent@example.com",
            ConfirmationCode = "1234",
            Password = "Newpassword123!"
        };
        
        var response = await _client.PostAsJsonAsync("/auth/reset-password", resetPasswordRequest);
        
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
    
    [Fact]
    public async Task TestBadRequest_NonValidPassword()
    {
      var resetPasswordRequest = new ResetPasswordRequest
      {
        Email = "business@example.com",
        ConfirmationCode = "1234",
        Password = "zxc"
      };
        
      var response = await _client.PostAsJsonAsync("/auth/reset-password", resetPasswordRequest);
        
      response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}
