using System.Net;
using System.Net.Http.Headers;

using FluentAssertions;

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

using reymani_web_api.Data;
using reymani_web_api.Data.Models;
using reymani_web_api.Services.BlobServices;

using ReymaniWebApi.Data.Models;

using Tests.Mock;

namespace Tests.Tests.Auth;

public class RegisterTests : IClassFixture<CustomWebApplicationFactory>
{
  private readonly CustomWebApplicationFactory _factory;
  private readonly HttpClient _client;

  public RegisterTests(CustomWebApplicationFactory factory)
  {
    _factory = factory;
    _client = _factory.CreateClient();
  }

  [Fact]
  public async Task TestOk_ValidRequest()
  {
    var formData = new MultipartFormDataContent();
    formData.Add(new StringContent("test@example.com"), "Email");
    formData.Add(new StringContent("Password123!"), "Password");
    formData.Add(new StringContent("Test"), "FirstName");
    formData.Add(new StringContent("User"), "LastName");
    formData.Add(new StringContent("1234567890"), "Phone");
    
    var response = await _client.PostAsync("/auth/register", formData);
    
    response.StatusCode.Should().Be(HttpStatusCode.OK);
    var responseContent = await response.Content.ReadAsStringAsync();
    responseContent.Should().Contain("User registered successfully");

    using var scope = _factory.Services.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    var user = await dbContext.Users.FirstOrDefaultAsync(u => u.Email == "test@example.com");
    user.Should().NotBeNull();
    user.FirstName.Should().Be("Test");
    user.LastName.Should().Be("User");
    user.Phone.Should().Be("1234567890");

    dbContext.Remove(user);
    await dbContext.SaveChangesAsync();
  }
  
  [Fact]
  public async Task TestOk_ValidRequest_WithProfilePhoto()
  {
    var env = _factory.Services.GetRequiredService<IWebHostEnvironment>();
    
    var formData = new MultipartFormDataContent();
    formData.Add(new StringContent("test@example.com"), "Email");
    formData.Add(new StringContent("Password123!"), "Password");
    formData.Add(new StringContent("Test"), "FirstName");
    formData.Add(new StringContent("User"), "LastName");
    formData.Add(new StringContent("1234567890"), "Phone");
    
    
    var basePath = env.ContentRootPath;
    var binIndex = basePath.IndexOf(@"\bin", StringComparison.Ordinal);
    if (binIndex > 0)
    {
      basePath = basePath.Substring(0, binIndex);
    }
    binIndex = basePath.IndexOf("/bin", StringComparison.Ordinal);
    if (binIndex > 0)
    {
      basePath = basePath.Substring(0, binIndex);
    }
    var imgPath = Path.Combine(basePath, "Files", "img.png");
    var imgContent = new ByteArrayContent(await File.ReadAllBytesAsync(imgPath));
    imgContent.Headers.ContentType = new MediaTypeHeaderValue("image/png");
    formData.Add(imgContent, "ProfilePicture", "img.png");
    
    var response = await _client.PostAsync("/auth/register", formData);
    
    response.StatusCode.Should().Be(HttpStatusCode.OK);
    var responseContent = await response.Content.ReadAsStringAsync();
    responseContent.Should().Contain("User registered successfully");
    
    using var scope = _factory.Services.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    var user = await dbContext.Users.FirstOrDefaultAsync(u => u.Email == "test@example.com");
    user.Should().NotBeNull();
    user.FirstName.Should().Be("Test");
    user.LastName.Should().Be("User");
    user.Phone.Should().Be("1234567890");

    dbContext.Remove(user);
    await dbContext.SaveChangesAsync();
  }

  [Fact]
  public async Task TestConflict_DuplicateEmail()
  {
    var firstFormData = new MultipartFormDataContent();
    firstFormData.Add(new StringContent("duplicate@example.com"), "Email");
    firstFormData.Add(new StringContent("Password123!"), "Password");
    firstFormData.Add(new StringContent("First"), "FirstName");
    firstFormData.Add(new StringContent("User"), "LastName");
    firstFormData.Add(new StringContent("1234567890"), "Phone");

    await _client.PostAsync("/auth/register", firstFormData);
    
    var secondFormData = new MultipartFormDataContent();
    secondFormData.Add(new StringContent("duplicate@example.com"), "Email");
    secondFormData.Add(new StringContent("AnotherPassword123!"), "Password");
    secondFormData.Add(new StringContent("Second"), "FirstName");
    secondFormData.Add(new StringContent("User"), "LastName");
    secondFormData.Add(new StringContent("0987654321"), "Phone");
    
    var response = await _client.PostAsync("/auth/register", secondFormData);
    response.StatusCode.Should().Be(HttpStatusCode.Conflict);
  }

  [Fact]
  public async Task TestBadRequest_InvalidEmail()
  {
    var formData = new MultipartFormDataContent();
    formData.Add(new StringContent("invalid-email"), "Email");
    formData.Add(new StringContent("Password123!"), "Password");
    formData.Add(new StringContent("Test"), "FirstName");
    formData.Add(new StringContent("User"), "LastName");
    formData.Add(new StringContent("1234567890"), "Phone");
    
    var response = await _client.PostAsync("/auth/register", formData);
    response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
  }

  [Fact]
  public async Task TestBadRequest_ShortPassword()
  {
    var formData = new MultipartFormDataContent();
    formData.Add(new StringContent("test2@example.com"), "Email");
    formData.Add(new StringContent("Short"), "Password");
    formData.Add(new StringContent("Test"), "FirstName");
    formData.Add(new StringContent("User"), "LastName");
    formData.Add(new StringContent("1234567890"), "Phone");
    
    var response = await _client.PostAsync("/auth/register", formData);
    response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
  }

  [Fact]
  public async Task TestBadRequest_MissingRequiredFields()
  {
    var formData = new MultipartFormDataContent();
    formData.Add(new StringContent("test3@example.com"), "Email");
    formData.Add(new StringContent("Test"), "FirstName");
    formData.Add(new StringContent("User"), "LastName");
    formData.Add(new StringContent("1234567890"), "Phone");
    
    var response = await _client.PostAsync("/auth/register", formData);
    response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
  }
  
  [Fact]
  public async Task TestBadRequest_NonValidProfilePhoto()
  {
    var env = _factory.Services.GetRequiredService<IWebHostEnvironment>();
    
    var formData = new MultipartFormDataContent();
    formData.Add(new StringContent("test@example.com"), "Email");
    formData.Add(new StringContent("Password123!"), "Password");
    formData.Add(new StringContent("Test"), "FirstName");
    formData.Add(new StringContent("User"), "LastName");
    formData.Add(new StringContent("1234567890"), "Phone");
    
    
    var basePath = env.ContentRootPath;
    var binIndex = basePath.IndexOf(@"\bin", StringComparison.Ordinal);
    if (binIndex > 0)
    {
      basePath = basePath.Substring(0, binIndex);
    }
    binIndex = basePath.IndexOf(@"/bin", StringComparison.Ordinal);
    if (binIndex > 0)
    {
      basePath = basePath.Substring(0, binIndex);
    }
    var imgPath = Path.Combine(basePath, "Files", "file.txt");
    var imgContent = new ByteArrayContent(await File.ReadAllBytesAsync(imgPath));
    imgContent.Headers.ContentType = new MediaTypeHeaderValue("text/plain");
    formData.Add(imgContent, "ProfilePicture", "img.png");
    
    var response = await _client.PostAsync("/auth/register", formData);
    
    response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
  }
}

