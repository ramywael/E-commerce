using System.ComponentModel.DataAnnotations;

namespace E_Commerce510.Models.ViewModel
{
    public class ProfileVm
    {
        [Required]
        public string userName { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        public string email { get; set; }

        [Required]
        public string address { get; set; }

        [DataType(DataType.Password)]
        public string? currentPassword { get; set; }

        [DataType(DataType.Password)]
        public string? newPassword { get; set; }

        [DataType(DataType.Password)]
        [Compare("newPassword",ErrorMessage = "New password and confirmation do not match .")]
        public string? confirmPassword { get; set; }

    }
}
