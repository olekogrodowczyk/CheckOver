using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CheckOver.Models.ViewModels
{
    public class AssignExerciseVM
    {
        [Required]
        [Display(Name = "Czas ukończenia")]
        public string DeadLineString { get; set; }
    }
}