using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CheckOver.Models.ViewModels
{
    public class ChangeGroupNameVM
    {
        [Required]
        [StringLength(50, MinimumLength = 5, ErrorMessage = "Nazwa grupy musi mieć minimalnie 5 znaków.")]
        [Display(Name = "Nazwa")]
        public string Name { get; set; }
    }
}