using System.ComponentModel.DataAnnotations;

namespace LibraryManagement.Models
{
    public class Admin
    {
        public string? Id { get; set; }
        
        [Required(ErrorMessage = "Username is required")]
        public string Username { get; set; } = "";
        
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; } = "";
        
        [EmailAddress]
        public string Email { get; set; } = "";
        
        public string FullName { get; set; } = "";
    }
}