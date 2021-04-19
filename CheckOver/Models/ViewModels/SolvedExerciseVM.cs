using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CheckOver.Models.ViewModels
{
    public class SolvedExerciseVM
    {
        public Solving Solving { get; set; }

        [Required]
        public string Answer { get; set; }
    }
}