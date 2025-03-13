using System;

namespace reymani_web_api.Services.EmailServices;

public class EmailTemplateService : IEmailTemplateService
{
  private readonly IWebHostEnvironment _env;
  
  public EmailTemplateService(IWebHostEnvironment env)
  {
    _env = env;
  }
  
  public async Task<string> GetTemplateAsync(string templateName, object model)
  {
    var basePath = _env.ContentRootPath;
    var binIndex = basePath.IndexOf(@"\bin", StringComparison.Ordinal);
    if (binIndex > 0)
    {
      basePath = basePath.Substring(0, binIndex);
    }
    binIndex = basePath.IndexOf("/bin", StringComparison.Ordinal);
    if (binIndex > 0)
    {
      basePath = basePath.Substring(0, binIndex);
    }
    string filePath = Path.Combine(basePath, "Services", "EmailServices", "Templates", $"{templateName}.html");

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