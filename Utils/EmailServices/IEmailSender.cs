using System;

namespace reymani_web_api.Utils.EmailServices;

public interface IEmailSender
{
  Task SendEmailAsync(string email, string subject, string message);
}
