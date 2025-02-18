using System;

namespace reymani_web_api.Services.EmailServices;

public interface IEmailSender
{
  Task SendEmailAsync(string email, string subject, string message);
}