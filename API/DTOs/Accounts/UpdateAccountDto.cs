using API.Utilities;
using System.ComponentModel.DataAnnotations;

namespace API.DTOs.Accounts
{
    public class UpdateAccountDto
    {
        [Required]
        public Guid Guid { get; set; }

        [PasswordPolicy]
        public string Password { get; set; }

        public bool IsDeleted { get; set; }

        public bool IsUsed { get; set; }
        public DateTime ExpiredTime { get; set; }
    }
}
