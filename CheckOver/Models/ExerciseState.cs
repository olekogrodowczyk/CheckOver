using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CheckOver.Models
{
    public class ExerciseState
    {
        public int Id { get; set; }
        public Assignment Assignment { get; set; }
        public string Description { get; set; }
        public int Points { get; set; }
        public Exercise Exercise { get; set; }
        public int ExerciseId { get; set; }
        public DateTime CreationTime { get; set; }
    }
}
