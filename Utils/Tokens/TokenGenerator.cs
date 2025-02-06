using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.IO;
using Microsoft.Extensions.Configuration;
using FastEndpoints.Security;
using Microsoft.IdentityModel.Tokens;
using ReymaniWebApi.Data.Models;

namespace reymani_web_api.Utils.Tokens
{
  public class TokenGenerator
  {
    public static string GenerateToken(User user)
    {
      var configuration = new ConfigurationBuilder()
          .SetBasePath(Directory.GetCurrentDirectory())
          .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
          .Build();
      var secretKey = configuration["JwtSecret"];
      if (string.IsNullOrEmpty(secretKey))
        throw new Exception("JwtSecret is not set");
      
      return JwtBearer.CreateToken(o =>
      {
        o.ExpireAt = DateTime.UtcNow.AddDays(2);
        o.SigningKey = secretKey;
        o.User.Roles.Add(user.Role.ToString());
        o.User.Claims.Add(("Email", user.Email));
      });

    }
  }
}
