using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CheckOver.Models.ViewModels
{
    public class SolvingsVM
    {
        public List<Solving> ToCheck { get; set; }
        public List<Solving> ToSolve { get; set; }
        public List<Solving> Checked { get; set; }
    }
}