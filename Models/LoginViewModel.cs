using System.ComponentModel.DataAnnotations;

namespace Crud_Application.Models
{
    // Represents the view model for the login functionality
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Email is required")] // Specifies that the email field is required
        [EmailAddress(ErrorMessage = "Invalid email address")] // Validates that the email field is a valid email address
        [StringLength(100, ErrorMessage = "Email cannot exceed 100 characters")] // Specifies the maximum length for the email field
        public string Email { get; set; } // Represents the email address input field

        [Required(ErrorMessage = "Password is required")] // Specifies that the password field is required
        [DataType(DataType.Password)] // Specifies that the password field is of type password
        [StringLength(100, ErrorMessage = "Password must be at least 6 characters", MinimumLength = 6)] // Specifies the minimum and maximum length for the password field
        public string Password { get; set; } // Represents the password input field
    }
}
