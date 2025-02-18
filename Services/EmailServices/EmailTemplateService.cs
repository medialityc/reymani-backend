using System;

namespace reymani_web_api.Services.EmailServices;

public class EmailTemplateService : IEmailTemplateService
{
  private readonly string _templatePath = Path.Combine(Directory.GetCurrentDirectory(), "Services", "EmailServices", "Templates");

  public async Task<string> GetTemplateAsync(string templateName, object model)
  {
    string filePath = Path.Combine(_templatePath, $"{templateName}.html");

    if (!File.Exists(filePath))
      throw new FileNotFoundException($"Template {templateName} not found at {filePath}");

    string template = await File.ReadAllTextAsync(filePath);

    return ReplacePlaceholders(template, model);
  }

  private string ReplacePlaceholders(string template, object model)
  {
    var properties = model.GetType().GetProperties();
    foreach (var prop in properties)
    {
      string placeholder = $"{{{{{prop.Name}}}}}"; // Ejemplo: {{FirstName}}
      string value = prop.GetValue(model)?.ToString() ?? "";
      template = template.Replace(placeholder, value);
    }
    return template;
  }
}