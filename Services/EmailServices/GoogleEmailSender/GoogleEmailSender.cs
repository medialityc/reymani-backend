using System;
using System.Net;
using System.Net.Mail;

using Microsoft.Extensions.Options;

using reymani_web_api.Utils.Options;
namespace reymani_web_api.Services.EmailServices.GoogleEmailSender;

public class GoogleEmailSender : IEmailSender
{
  private readonly GoogleEmailSenderOptions _options;

  public GoogleEmailSender(IOptions<GoogleEmailSenderOptions> optionsAccessor)
  {
    _options = optionsAccessor.Value;
  }

  public Task SendEmailAsync(string email, string subject, string htmlMessage)
  {
    var client = new SmtpClient
    {
      Host = _options.Host,
      Port = _options.Port,
      EnableSsl = _options.EnableSsl,
      DeliveryMethod = SmtpDeliveryMethod.Network,
      UseDefaultCredentials = false,
      Credentials = new NetworkCredential(_options.User, _options.Password)
    };

    var message = new MailMessage
    {
      From = new MailAddress(_options.SenderEmail, _options.SenderName),
      Subject = subject,
      Body = htmlMessage,
      IsBodyHtml = true
    };

    message.To.Add(new MailAddress(email));

    return client.SendMailAsync(message);
  }
}