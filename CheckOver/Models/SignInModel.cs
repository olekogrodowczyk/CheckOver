using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CheckOver.Models
{
    public class SignInModel
    {
        [Required(ErrorMessage = "Proszę podać adres e-mail"), EmailAddress]
        [Display(Name = "Adres e-mail")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Proszę podać hasło")]
        [DataType(DataType.Password)]
        [Display(Name = "Hasło")]
        public string Password { get; set; }
        [Display(Name ="Remember me")]
        public bool RememberMe { get; set; }
    }
}
