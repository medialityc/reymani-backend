using Microsoft.Extensions.Options;

using Minio;
using Minio.DataModel.Args;

using reymani_web_api.Utils.Options;

namespace reymani_web_api.Services.BlobServices.Minio
{
  public class MinioBlobService : IBlobService
  {
    private readonly string _bucketName;
    private readonly IMinioClient _minioClient;

    public MinioBlobService(IOptions<MinioOptions> options)
    {
      _bucketName = options.Value.Bucket;
      _minioClient = new MinioClient()
          .WithEndpoint(options.Value.Endpoint, options.Value.Port)
          .WithCredentials(options.Value.AccessKey, options.Value.SecretKey)
          .WithSSL(false)
          .Build();
    }

    public async Task<string> UploadObject(IFormFile file, string codeObj, CancellationToken ct)
    {
      if (file == null || file.Length == 0)
        throw new ArgumentException("Archivo inválido o vacío.");

      var ext = Path.GetExtension(file.FileName).ToLower();
      if (string.IsNullOrEmpty(ext))
        throw new InvalidOperationException("No se pudo determinar la extensión del archivo.");

      var objectPath = $"images/{codeObj}{ext}";

      try
      {
        await using var ms = new MemoryStream();
        await file.CopyToAsync(ms, ct);
        ms.Position = 0;

        var uploadArgs = new PutObjectArgs()
            .WithBucket(_bucketName)
            .WithObject(objectPath)
            .WithStreamData(ms)
            .WithObjectSize(ms.Length)
            .WithContentType(file.ContentType);

        await _minioClient.PutObjectAsync(uploadArgs, ct);
      }
      catch
      {
        throw new ApplicationException("No se pudo subir el archivo al almacenamiento.");
      }

      return objectPath;
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

    // Nueva funcionalidad para eliminar un objeto dado su path.
    public async Task DeleteObject(string objPath, CancellationToken ct)
    {
      var args = new RemoveObjectArgs()
          .WithBucket(_bucketName)
          .WithObject(objPath);

      await _minioClient.RemoveObjectAsync(args, ct);
    }
  }
}
