using System;
using System.Security.Cryptography;
using System.Text;

namespace reymani_web_api.Application.Utils;

public static class HashPassword
{
  public static string ComputeHash(string password)
  {
    using (var sha256 = SHA256.Create())
    {
      var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
      return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
    }
  }

  public static bool VerifyHash(string password, string hash)
  {
    return ComputeHash(password) == hash;
  }
}
