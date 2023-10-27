using System.ComponentModel.DataAnnotations;

namespace MyAnimeVault.Models
{
    public class LoginViewModel
    {
        public LoginViewModel()
        {
            Email = string.Empty;
            Password = string.Empty;
        }

        [Required(ErrorMessage = "Please enter your email address.")]
        public string Email { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Please enter your password.")]
        public string Password { get; set; }
    }
}
