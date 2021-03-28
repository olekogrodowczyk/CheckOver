using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CheckOver.Models
{
    public class SignUpUserModel
    {
        [Required(ErrorMessage = "Proszę podać nazwisko")]
        [Display(Name = "Imię")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Proszę podać imię")]
        [Display(Name = "Nazwisko")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Proszę podać adres e-mail")]
        [Display(Name = "Adres e-mail")]
        [EmailAddress(ErrorMessage = "Proszę podać prawidłowy adres e-mail")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Proszę podać hasło")]
        [Compare("ConfirmPassword",ErrorMessage = "Hasła nie zgadzają się")]
        [Display(Name = "Hasło")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required(ErrorMessage = "Proszę potwierdzić hasło")]
        [Display(Name = "Potwierdź hasło")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }
}
