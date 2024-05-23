
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
namespace Pochemychka.ViewModels
{
    public class LoginAndRegisterModel
    {

        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string? Password { get; set; }


        [Display(Name = "Login")]
        public string? Name { get; set; }

        [Display(Name = "Email")]
        public string? Email { get; set; }
        [Required]
        [Display(Name = "FirstName")]
        public string FirstName { get; set; }
        [Required]
        [Display(Name = "LastName")]
        public string LastName { get; set; }
        [Compare("Password", ErrorMessage = "Пароли не совподают")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        public string? PasswordConfirm { get; set; }

        public string? Role { get; set; }
    }

}
