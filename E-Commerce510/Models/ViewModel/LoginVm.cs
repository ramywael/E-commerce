using System.ComponentModel.DataAnnotations;

namespace E_Commerce510.Models.ViewModel
{
    public class LoginVm
    {
        public int Id { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }
}
