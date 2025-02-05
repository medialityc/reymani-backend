using System;

namespace ReymaniWebApi.Data.Models
{
    public class ConfirmationNumber
    {
        public int Id { get; set; }
        public string Number { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}