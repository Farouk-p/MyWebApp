using System.ComponentModel.DataAnnotations;

namespace MyWebApp.Models
{
    public class UserProfile
    {
        [Key]
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string Lastname { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Phone { get; set; }
        public string UserId { get; set; }
        public DateTime CreatedDate { get; set; }
        public string IsActive { get; set; }
        public string Status { get; set; }
    }
}
