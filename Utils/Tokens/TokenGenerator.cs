using FastEndpoints.Security;

using Microsoft.Extensions.Options;

using reymani_web_api.Utils.Options;

using ReymaniWebApi.Data.Models;

namespace reymani_web_api.Utils.Tokens
{
  public class TokenGenerator
  {
    private readonly AuthOptions _authOptions;

    // Constructor con inyección de IOptions<AuthOptions>
    public TokenGenerator(IOptions<AuthOptions> options)
    {
      _authOptions = options.Value;
    }

    public string GenerateToken(User user)
    {
      var secretKey = _authOptions.JwtToken;
      if (string.IsNullOrEmpty(secretKey))
        throw new Exception("JwtToken is not set in AuthOptions");

      return JwtBearer.CreateToken(o =>
      {
        o.ExpireAt = DateTime.UtcNow.AddDays(2);
        o.SigningKey = secretKey;
        o.User.Roles.Add(user.Role.ToString());
        o.User.Claims.Add(("Id", user.Id.ToString()));
      });
    }
  }
}