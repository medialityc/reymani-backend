using Microsoft.EntityFrameworkCore;
using FastEndpoints.Security;
using reymani_web_api;

var bld = WebApplication.CreateBuilder();
bld.Services
   .AddAuthenticationJwtBearer(s => s.SigningKey = bld.Configuration["jwtsecret"])
   .AddAuthorization()
   .AddFastEndpoints()
   .SwaggerDocument(); //define a swagger document

bld.Services.AddDbContextFactory<DBContext>(options =>
    options.UseNpgsql(bld.Configuration.GetConnectionString("DefaultConnection")));

bld.Services.AddDomainServices();
bld.Services.AddRepositories();

var app = bld.Build();

// Apply migrations and seed the database
using (var scope = app.Services.CreateScope())
{
   var dbContext = scope.ServiceProvider.GetRequiredService<DBContext>();
   dbContext.Database.Migrate();
   SeedData.SeedRoles(dbContext);
   SeedData.SeedPermisos(dbContext);
}

app.UseDefaultExceptionHandler()
   .UseAuthentication()
   .UseAuthorization()
   .UseFastEndpoints()
   .UseSwaggerGen(); //add this

app.Run();
