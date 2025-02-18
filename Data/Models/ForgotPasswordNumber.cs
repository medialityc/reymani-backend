using System;

using ReymaniWebApi.Data.Models;

namespace reymani_web_api.Data.Models;

public class ForgotPasswordNumber : BaseEntity
{
  public int Id { get; set; }
  public required string Number { get; set; }
  public required int UserId { get; set; }
  public User? User { get; set; }
}