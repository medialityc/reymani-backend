using System.Net;
using System.Net.Http.Json;

using FluentAssertions;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

using reymani_web_api.Data;
using reymani_web_api.Endpoints.Auth.Requests;

using ReymaniWebApi.Data.Models;

using Tests.Mock;

namespace Tests.Tests.Auth;

public class ConfirmTests(CustomWebApplicationFactory factory) : IClassFixture<CustomWebApplicationFactory>
{
  private readonly HttpClient _client = factory.CreateClient();

  // [Fact]
  // public async Task TestOk_ConfirmUserSuccessfully()
  // {
  //   using var scope = factory.Services.CreateScope();
  //   var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
  //
  //   var confirmRequest = new ConfirmEndpointRequest { Email = "unconfirmed@example.com", ConfirmationCode = "1234" };
  //
  //   var response = await _client.PostAsJsonAsync("/auth/confirm", confirmRequest);
  //
  //   response.StatusCode.Should().Be(HttpStatusCode.OK);
  //
  //   var content = await response.Content.ReadAsStringAsync();
  //   content.Should().Be("\"User confirmed successfully\"");
  //
  //   var user = await dbContext.Users.FirstOrDefaultAsync(x => x.Email == confirmRequest.Email);
  //   user.Should().NotBeNull();
  //   user.IsConfirmed.Should().BeTrue();
  // }

  [Fact]
  public async Task TestNotFound_UserDoesNotExist()
  {
    var confirmRequest = new ConfirmEndpointRequest { Email = "nonexistent@example.com", ConfirmationCode = "1234" };
  
    var response = await _client.PostAsJsonAsync("/auth/confirm", confirmRequest);
  
    response.StatusCode.Should().Be(HttpStatusCode.NotFound);
  }
  
  // [Fact]
  // public async Task TestBadRequest_InvalidConfirmationCode()
  // {
  //   var confirmRequest = new ConfirmEndpointRequest { Email = "unconfirmed@example.com", ConfirmationCode = "0000" };
  //
  //   var response = await _client.PostAsJsonAsync("/auth/confirm", confirmRequest);
  //
  //   response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
  // }
}