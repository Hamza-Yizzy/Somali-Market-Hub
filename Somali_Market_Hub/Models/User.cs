// File: Models/User.cs
using System;
using System.ComponentModel.DataAnnotations;

namespace Somali_Market_Hub.Models
{
    public class User
    {
        [Key]
        public string Id { get; set; } = null!;

        [Required]
        public string FullName { get; set; } = null!;

        [Required, EmailAddress]
        [RegularExpression(@"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "Please Enter Valid Email.")]
        public string Email { get; set; } = null!;

        [Required]
        public string Username { get; set; } = null!;

        [Required]
        public string Password { get; set; } = null!;

        [Required]
        public string Role { get; set; } = null!; // Admin, Provider, Customer

        public string? BusinessName { get; set; } // Only for Providers
        public string? BusinessLogo { get; set; } // Only for Providers
        public string? Location { get; set; } // Only for Providers
        public string? DeliveryAvailable { get; set; } // Yes or No for Providers
    }
}
