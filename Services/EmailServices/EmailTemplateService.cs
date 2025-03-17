using System;

using reymani_web_api.Services.EmailServices.Templates;

namespace reymani_web_api.Services.EmailServices;

public class EmailTemplateService(IWebHostEnvironment env) : IEmailTemplateService
{
  private readonly ConfirmationEmailTemplate _confirmationEmailTemplate = new ConfirmationEmailTemplate();
  private readonly PasswordResetEmailTemplate _passwordResetEmailTemplate = new PasswordResetEmailTemplate();

  public string GetTemplateAsync(TemplateName templateName, object model)
  {
    switch (templateName)
    {
      case TemplateName.Confirmation:
        return ReplacePlaceholders(_confirmationEmailTemplate.GetEmailTemplate(), model);
      case TemplateName.ResetPassword:
        return ReplacePlaceholders(_passwordResetEmailTemplate.GetEmailTemplate(), model);
    }
    return string.Empty;
  }

  private string ReplacePlaceholders(string template, object model)
  {
    var properties = model.GetType().GetProperties();
    foreach (var prop in properties)
    {
      string placeholder = $"{{{{{prop.Name}}}}}";
      string value = prop.GetValue(model)?.ToString() ?? "";
      template = template.Replace(placeholder, value);
    }
    return template;
  }
}