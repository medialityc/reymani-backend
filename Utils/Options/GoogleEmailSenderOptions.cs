using System;

namespace reymani_web_api.Utils.Options;

public class GoogleEmailSenderOptions
{
  public string Host { get; set; } = default!;
  public int Port { get; set; }
  public string User { get; set; } = default!;
  public string Password { get; set; } = default!;
  public bool EnableSsl { get; set; }
  public string SenderName { get; set; } = default!;
  public string SenderEmail { get; set; } = default!;
}
