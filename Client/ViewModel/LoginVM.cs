using API.Utilities;
using System.ComponentModel.DataAnnotations;

namespace Client.ViewModel
{
    public class LoginVM
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [PasswordPolicy]
        public string Password { get; set; }
    }
}
