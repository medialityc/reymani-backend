using Microsoft.Extensions.Options;

using Minio;
using Minio.DataModel.Args;

using reymani_web_api.Utils.Validations;

namespace reymani_web_api.Services.BlobServices.Minio;

public class MinioBlobService : IBlobService
{
  private readonly string _bucketName;
  private readonly IMinioClient _minioClient;

  public MinioBlobService(IOptions<MinioOptions> options)
  {
    _bucketName = options.Value.Bucket;
    _minioClient = new MinioClient()
        .WithEndpoint(options.Value.Endpoint)
        .WithCredentials(options.Value.AccessKey, options.Value.SecretKey)
        .WithSSL(false)
        .Build();
  }

  public async Task UploadObject(IFormFile file, string codeObj, CancellationToken ct)
  {
    // Validar si el archivo es una imagen válida y tiene contenido
    if (!ImageValidations.BeAValidImage(file))
      throw new InvalidOperationException("El archivo no es un tipo de imagen permitido.");

    if (!ImageValidations.HaveValidLength(file))
      throw new InvalidOperationException("El archivo está vacío o no es válido.");

    // Obtener la extensión real del archivo
    var ext = Path.GetExtension(file.FileName).ToLower();
    if (string.IsNullOrEmpty(ext))
      throw new InvalidOperationException("No se pudo determinar la extensión del archivo.");

    // Ruta donde se almacenará en Minio
    var objectPath = $"images/{codeObj}{ext}";

    await using var fileStream = file.OpenReadStream();
    var uploadArgs = new PutObjectArgs()
        .WithBucket(_bucketName)
        .WithObject(objectPath)
        .WithStreamData(fileStream)
        .WithObjectSize(fileStream.Length)
        .WithContentType(file.ContentType);

    await _minioClient.PutObjectAsync(uploadArgs, ct);
  }

  public async Task<string> PresignedGetUrl(string objPath, CancellationToken ct)
  {
    var args = new PresignedGetObjectArgs()
        .WithBucket(_bucketName)
        .WithObject(objPath)
        .WithExpiry(60 * 5);
    return await _minioClient.PresignedGetObjectAsync(args);
  }

  public async Task<bool> ValidateExistanceObject(string objPath, CancellationToken ct)
  {
    try
    {
      var statObjectArgs = new StatObjectArgs()
          .WithBucket(_bucketName)
          .WithObject(objPath);

      await _minioClient.StatObjectAsync(statObjectArgs, ct);
      return true;
    }
    catch
    {
      return false;
    }
  }
}
