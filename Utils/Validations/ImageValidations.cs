using System;

namespace reymani_web_api.Utils.Validations
{
  public static class ImageValidations
  {
    public static bool BeAValidImage(IFormFile? file)
    {
      if (file == null)
        return true;

      var validImageTypes = new[]
      {
        "image/jpeg", "image/png", "image/gif", "image/bmp", "image/webp", "image/avif",
        "image/jjif"
      };
      return validImageTypes.Contains(file.ContentType);
    }

    public static bool HaveValidLength(IFormFile? file)
    {
      if (file == null) return true;
      return file.Length > 0;
    }

    public static bool HaveValidImages(List<IFormFile>? images)
    {
      if (images == null || !images.Any())
        return true;

      foreach (var image in images)
      {
        if (!BeAValidImage(image) || !HaveValidLength(image))
          return false;
      }

      return true;
    }
  }
}