using System;

using reymani_web_api.Services.EmailServices.Templates;

namespace reymani_web_api.Services.EmailServices;

public interface IEmailTemplateService
{
  string GetTemplateAsync(TemplateName templateName, object model);
}