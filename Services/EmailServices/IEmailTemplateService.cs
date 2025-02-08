using System;

namespace reymani_web_api.Services.EmailServices;

public interface IEmailTemplateService
{
  Task<string> GetTemplateAsync(string templateName, object model);
}
