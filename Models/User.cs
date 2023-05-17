using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Crud_Application.Models
{
    // Represents a user in the application
    public class User
    {
        public int Id { get; set; } // The ID of the user

        [Required(ErrorMessage = "First name is required")]
        [StringLength(50, ErrorMessage = "First name cannot exceed 50 characters")]
        public string FirstName { get; set; } // The first name of the user

        [Required(ErrorMessage = "Last name is required")]
        [StringLength(50, ErrorMessage = "Last name cannot exceed 50 characters")]
        public string LastName { get; set; } // The last name of the user

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        [StringLength(100, ErrorMessage = "Email cannot exceed 100 characters")]
        public string Email { get; set; } // The email address of the user

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        [StringLength(100, ErrorMessage = "Password must be at least 6 characters", MinimumLength = 6)]
        public string Password { get; set; } // The password of the user

        [DataType(DataType.DateTime)]
        public DateTime CreatedAt { get; set; } // The date and time when the user was created

        [DataType(DataType.DateTime)]
        public DateTime? UpdatedAt { get; set; } // The date and time when the user was last updated (nullable)
    }
}
