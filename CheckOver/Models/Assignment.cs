using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CheckOver.Models
{
    public class Assignment
    {
        public int Id { get; set; }
        public ApplicationUser User { get; set; }
        public Role Role { get; set; }
        public Group Group { get; set; }
        public ICollection<ExerciseState> ExerciseStates { get; set; }
    }
}
