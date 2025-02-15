using FastEndpoints.Security;
using ReymaniWebApi.Data.Models;
using reymani_web_api.Utils.Options;
using Microsoft.Extensions.Options;

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

    public virtual string GenerateToken(User user)
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
