using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CheckOver.Models.ViewModels
{
    public class GetGroupVM
    {
        public Group Group { get; set; }
        public List<Assignment> Checkers { get; set; }
        public List<Assignment> Solvers { get; set; }
        public List<Exercise> Exercises { get; set; }
    }
}