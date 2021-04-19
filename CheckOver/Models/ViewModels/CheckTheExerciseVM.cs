using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CheckOver.Models.ViewModels
{
    public class CheckTheExerciseVM
    {
        public Solving Solving { get; set; }

        public string Remarks { get; set; }

        [Required]
        [Display(Name = "Liczba punktów")]
        public int Points { get; set; }
    }
}