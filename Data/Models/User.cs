using System;
using reymani_web_api.Data.Models;

namespace ReymaniWebApi.Data.Models
{
    public class User
    {
        public int Id { get; set; }
        public string? ProfilePicture { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }        
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Password { get; set; }
        public bool IsActive { get; set; }
        public UserRole Role { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}