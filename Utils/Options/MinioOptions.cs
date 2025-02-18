using System;

namespace reymani_web_api.Utils.Options;

public class MinioOptions
{
  public required string Endpoint { get; set; }
  public required string AccessKey { get; set; }
  public required string SecretKey { get; set; }
  public required string Bucket { get; set; }
  public required int Port { get; set; }

}