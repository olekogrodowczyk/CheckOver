using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CheckOver.Models.ViewModels
{
    public class InvitationVM
    {
        [Required(ErrorMessage = "Proszę podać adres e-mail użytkownika"), EmailAddress]
        [Display(Name = "Adres e-mail")]
        public string Email { get; set; }

        [Display(Name = "Rola")]
        public string Role { get; set; }
    }
}