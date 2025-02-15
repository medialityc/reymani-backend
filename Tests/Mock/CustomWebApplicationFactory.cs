using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;

namespace Tests.Mock;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
  protected override void ConfigureWebHost(IWebHostBuilder builder)
  {
    builder.ConfigureServices(services =>
    {
      services.AddScoped(_ => Mocker.BuildMockAppDbContext());
      services.AddScoped(_ => Mocker.BuildMockIBlobService());
      services.AddScoped(_ => Mocker.BuildMockEmailSender());
      services.AddScoped(_ => Mocker.BuildMockEmailTemplateService());
      services.AddScoped(_ => Mocker.BuildMockTokenGenerator());
    });
  }
}