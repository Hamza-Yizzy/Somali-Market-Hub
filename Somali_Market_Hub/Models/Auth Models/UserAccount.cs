using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Somali_Market_Hub.Models
{
    public class UserAccount
    {
        [Required] public int Id { get; set; }
        [Required] public string FullName { get; set; } = null!;

        [RegularExpression(@"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "Please Enter Valid Email.")]
        [Required] public string Email { get; set; } = null!;
        [Required] public string UserName { get; set; } = null!;
        [Required] public string Password { get; set; } = null!;

        public int RoleId { get; set; }
        [ForeignKey("RoleId")]
        public Role? Roles { get; set; }

        public string BusinessName { get; set; } = null!;
        public byte[]? BusinessLogo { get; set; }
        public string Location { get; set; } = null!;
    }
}
