using System.ComponentModel.DataAnnotations;

namespace LibraryManagement.Models
{
    public class Member
    {
        public string? Id { get; set; }
        
        [Required(ErrorMessage = "Name is required")]
        [Display(Name = "Full Name")]
        public string Name { get; set; } = "";
        
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        [Display(Name = "Email Address")]
        public string Email { get; set; } = "";
        
        [Required(ErrorMessage = "Phone is required")]
        [Phone(ErrorMessage = "Invalid phone number")]
        [Display(Name = "Phone Number")]
        public string Phone { get; set; } = "";
        
        [Display(Name = "Address")]
        public string Address { get; set; } = "";
        
        [Display(Name = "Membership Number")]
        public string MembershipNumber { get; set; } = "";
        
        [Display(Name = "Join Date")]
        public DateTime JoinDate { get; set; }
        
        [Display(Name = "Status")]
        public string Status { get; set; } = "Active";
        
        [Display(Name = "Member Type")]
        public string MemberType { get; set; } = "Regular";
    }
}