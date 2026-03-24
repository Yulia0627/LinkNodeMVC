using System.ComponentModel.DataAnnotations;

namespace LinkNodeInfrastructure.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
        [Display(Name = "Email")]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Ім'я")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Прізвище")]
        public string Surname { get; set; }

        [Required]
        [Display(Name = "Країна")]
        public string Country { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }

        [Required]
        [Compare("Password", ErrorMessage = "Паролі не збігаються")]
        [DataType(DataType.Password)]
        [Display(Name = "Підтвердження пароля")]
        public string PasswordConfirm { get; set; }
    }
}
