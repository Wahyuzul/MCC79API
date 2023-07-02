using API.Utilities;
using System.ComponentModel.DataAnnotations;

namespace API.DTOs.Accounts
{
    public class LoginAccountDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [PasswordPolicy]
        public string Password { get; set; }
    }
}
