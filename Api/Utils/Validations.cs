using System;
using System.Globalization;

namespace reymani_web_api.Api.Utils;

public static class Validations
{
  public static bool BeAValidDate(string numeroCarnet)
  {
    if (numeroCarnet.Length < 6)
      return false;

    var datePart = numeroCarnet.Substring(0, 6);
    if (DateTime.TryParseExact(datePart, "yyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var date))
    {
      return date < DateTime.Now;
    }
    return false;
  }
}
