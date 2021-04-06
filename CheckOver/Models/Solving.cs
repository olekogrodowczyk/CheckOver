using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CheckOver.Models
{
    public class Solving
    {
        [Key]
        public int SolvingId { get; set; }

        [ForeignKey("Checking")]
        public int CheckingId { get; set; }

        public virtual Checking Checking { get; set; }

        [ForeignKey("Exercise")]
        public int ExerciseId { get; set; }

        public virtual Exercise Exercise { get; set; }
        public string Status { get; set; }
        public string ProgrammingLanguage { get; set; }
        public DateTime CreatedAt { get; set; }

        [ForeignKey("Assignment")]
        public int AssignmentId { get; set; }

        public virtual Assignment Assignment { get; set; }
    }
}