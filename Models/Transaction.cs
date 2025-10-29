using System.ComponentModel.DataAnnotations;

namespace LibraryManagement.Models
{
    public class Transaction
    {
        public string? Id { get; set; }
        
        public string BookId { get; set; } = "";
        
        public string MemberId { get; set; } = "";
        
        [Display(Name = "Book Title")]
        public string BookTitle { get; set; } = "";
        
        [Display(Name = "Member Name")]
        public string MemberName { get; set; } = "";
        
        [Display(Name = "Issue Date")]
        public DateTime IssueDate { get; set; }
        
        [Display(Name = "Due Date")]
        public DateTime DueDate { get; set; }
        
        [Display(Name = "Return Date")]
        public DateTime? ReturnDate { get; set; }
        
        [Display(Name = "Status")]
        public string Status { get; set; } = "Issued";
        
        [Display(Name = "Fine Amount")]
        public decimal Fine { get; set; } = 0;
        
        [Display(Name = "Notes")]
        public string Notes { get; set; } = "";
    }
}