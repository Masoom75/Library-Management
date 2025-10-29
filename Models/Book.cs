using System.ComponentModel.DataAnnotations;

namespace LibraryManagement.Models
{
    public class Book
    {
        public string? Id { get; set; }
        
        [Required(ErrorMessage = "Title is required")]
        [Display(Name = "Book Title")]
        public string Title { get; set; } = "";
        
        [Required(ErrorMessage = "Author is required")]
        [Display(Name = "Author Name")]
        public string Author { get; set; } = "";
        
        [Required(ErrorMessage = "ISBN is required")]
        [Display(Name = "ISBN Number")]
        public string ISBN { get; set; } = "";
        
        [Display(Name = "Category")]
        public string Category { get; set; } = "";
        
        [Required]
        [Range(1, 1000, ErrorMessage = "Quantity must be between 1 and 1000")]
        [Display(Name = "Total Quantity")]
        public int Quantity { get; set; }
        
        [Display(Name = "Available Quantity")]
        public int AvailableQuantity { get; set; }
        
        public string? ImagePath { get; set; }
        
        [Display(Name = "Added Date")]
        public DateTime AddedDate { get; set; }
        
        [Display(Name = "Publisher")]
        public string Publisher { get; set; } = "";
        
        [Display(Name = "Year Published")]
        public int YearPublished { get; set; }
    }
}