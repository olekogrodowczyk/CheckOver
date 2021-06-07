using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CheckOver.Models.ViewModels
{
    public class ConfigureExerciseVM
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public int MaxPoints { get; set; }
        public string ValidCode { get; set; }
        public string Arguments { get; set; }
    }
}