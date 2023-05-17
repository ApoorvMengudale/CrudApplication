using System.ComponentModel.DataAnnotations;

namespace Crud_Application.Models
{
    // Represents a view model for creating or editing an entry
    public class CreateEditEntryViewModel
    {
        public int Id { get; set; } // The unique identifier for the entry

        [Required(ErrorMessage = "Account is required")]
        [StringLength(100, ErrorMessage = "Account cannot exceed 100 characters")]
        public string Account { get; set; } // The account associated with the entry

        [Required(ErrorMessage = "Narration is required")]
        [StringLength(100, ErrorMessage = "Narration cannot exceed 100 characters")]
        public string Narration { get; set; } // The description or narration of the entry

        [Required(ErrorMessage = "Currency is required")]
        [StringLength(50, ErrorMessage = "Currency cannot exceed 50 characters")]
        public string Currency { get; set; } // The currency of the entry

        [Required(ErrorMessage = "Type is required")]
        public string Type { get; set; } // The type or category of the entry

        [Required(ErrorMessage = "Balance is required")]
        public decimal Amount { get; set; } // The amount or balance of the entry
    }
}
