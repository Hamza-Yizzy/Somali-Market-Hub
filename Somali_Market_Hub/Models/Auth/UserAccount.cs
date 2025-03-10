using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Somali_Market_Hub.Models // ✅ Correct namespace
{
    [Index(nameof(Id), IsUnique = true)]
    public class UserAccount
    {
        [Required]
        public string Id { get; set; } = null!; // Make it a string now (e.g., "SA01")

        [Required]
        public string FullName { get; set; } = null!;

        [RegularExpression(@"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$",
        ErrorMessage = "Please Enter Valid Email.")]
        [Required]
        public string Email { get; set; } = null!;

        [Required]
        public string UserName { get; set; } = null!;

        [Required]
        public string Password { get; set; } = null!;

        public int RoleId { get; set; }
        [ForeignKey("RoleId")]
        public Role? Roles { get; set; }

        public string BusinessName { get; set; } = null!;
        public byte[]? BusinessLogo { get; set; }
        public string Location { get; set; } = null!;
    }

}
