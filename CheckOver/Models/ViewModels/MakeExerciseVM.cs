using CustomDataAnnotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CheckOver.Models.ViewModels
{
    public class MakeExerciseVM
    {
        [Required]
        [Display(Name = "Tytuł")]
        public string Title { get; set; }

        [Required]
        [Display(Name = "Zadanie")]
        [MinLength(10)]
        [MaxLength(2000)]
        public string Description { get; set; }

        [Required]
        [Display(Name = "Maksymalna liczba punktów")]
        public int MaxPoints { get; set; }

        [Required]
        [Display(Name = "Czas ukończenia")]
        public string DeadLineString { get; set; }
    }
}