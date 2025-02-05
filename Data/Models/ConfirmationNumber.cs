using System;

using reymani_web_api.Data.Models;

namespace ReymaniWebApi.Data.Models
{
  public class ConfirmationNumber : BaseEntity
  {
    public int Id { get; set; }
    public required string Number { get; set; }
    public required int UserId { get; set; }
    public User? User { get; set; }
  }
}