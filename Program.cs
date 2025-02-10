using System.Text.Json.Serialization;

using FastEndpoints;
using FastEndpoints.Security;
using FastEndpoints.Swagger;

using Microsoft.EntityFrameworkCore;

using reymani_web_api.Data;
using reymani_web_api.Services.BlobServices;
using reymani_web_api.Services.BlobServices.Minio;
using reymani_web_api.Services.EmailServices;
using reymani_web_api.Services.EmailServices.GoogleEmailSender;
using reymani_web_api.Utils.Seeders;

var bld = WebApplication.CreateBuilder();
bld.Services
  .AddCors(options =>
    {
      options.AddPolicy("AllowAll", builder =>
        {
          builder.AllowAnyOrigin()
                 .AllowAnyMethod()
                 .AllowAnyHeader();
        });
    })
  .AddAuthenticationJwtBearer(s => s.SigningKey = bld.Configuration["jwtsecret"])
  .AddAuthorization()
  .AddFastEndpoints()
  .SwaggerDocument(); //define a swagger document

bld.Services.AddDbContextFactory<AppDbContext>(options =>
  options.UseNpgsql(
    bld.Configuration.GetConnectionString("DefaultConnection")
  ));


bld.Services.Configure<GoogleEmailSenderOptions>(bld.Configuration.GetSection("GoogleEmailSender"));
bld.Services.AddSingleton<IEmailSender, GoogleEmailSender>();

bld.Services.Configure<MinioOptions>(bld.Configuration.GetSection("Minio"));
bld.Services.AddScoped<IBlobService, MinioBlobService>();
bld.Services.AddScoped<IEmailTemplateService, EmailTemplateService>();


bld.Services.AddControllers().AddJsonOptions(options =>
{
  options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
});

var app = bld.Build();

// Apply migrations and seed the database
using (var scope = app.Services.CreateScope())
{
  var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
  dbContext.Database.Migrate();
  SeedData.Initialize(scope.ServiceProvider);
}

app.UseCors("AllowAll")
  .UseDefaultExceptionHandler()
  .UseAuthentication()
  .UseAuthorization()
  .UseFastEndpoints()
  .UseSwaggerGen(); //add this

app.MapGet("/", context =>
{
  context.Response.Redirect("/swagger");
  return Task.CompletedTask;
});

app.Run();