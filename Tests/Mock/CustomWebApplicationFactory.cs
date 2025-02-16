using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Tests.Mock;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
  protected override void ConfigureWebHost(IWebHostBuilder builder)
  {
    builder.ConfigureServices(services =>
    {
      var sp = services.BuildServiceProvider();
      var configuration = sp.GetRequiredService<IConfiguration>();
      
      services.AddScoped(_ => Mocker.BuildMockAppDbContext());
      services.AddScoped(_ => Mocker.BuildMockIBlobService());
      services.AddScoped(_ => Mocker.BuildMockEmailSender());
      services.AddScoped(_ => Mocker.BuildMockEmailTemplateService());
      services.AddScoped(_ => Mocker.BuildMockTokenGenerator(configuration));
      
    });
  }
}