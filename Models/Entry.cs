using System.ComponentModel.DataAnnotations;

namespace Crud_Application.Models
{
    // Represents an entry in the application
    public class Entry
    {
        public int Id { get; set; } // The ID of the entry

        [Required(ErrorMessage = "Account is required")]
        [StringLength(100, ErrorMessage = "Account cannot exceed 100 characters")]
        public string Account { get; set; } // The account associated with the entry

        [Required(ErrorMessage = "Narration is required")]
        [StringLength(100, ErrorMessage = "Narration cannot exceed 100 characters")]
        public string Narration { get; set; } // The narration or description of the entry

        [Required(ErrorMessage = "Currency is required")]
        [StringLength(50, ErrorMessage = "Currency cannot exceed 50 characters")]
        public string Currency { get; set; } // The currency of the entry

        [Required(ErrorMessage = "Type is required")]
        public string Type { get; set; } // The type of transaction (credit or debit)

        [Required(ErrorMessage = "Balance is required")]
        public decimal Balance { get; set; } // The balance or amount associated with the entry

        [DataType(DataType.DateTime)]
        public DateTime CreatedAt { get; set; } // The date and time when the entry was created

        [DataType(DataType.DateTime)]
        public DateTime? UpdatedAt { get; set; } // The date and time when the entry was last updated (nullable)

        public User User { get; set; } // The user associated with the entry
    }

    // Represents the type of transaction (credit or debit)
    public enum TransactionType
    {
        Credit,
        Debit
    }

    // Represents the type of account (savings, salary, current)
    public enum AccountType
    {
        Savings,
        Salary,
        Current
    }

    // Represents the available currencies
    public enum Currency
    {
        INR,
        USD
    }
}
