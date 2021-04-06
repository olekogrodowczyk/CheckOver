﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CheckOver.Models
{
    public class Exercise
    {
        [Key]
        public int ExerciseId { get; set; }

        public string Title { get; set; }
        public string Description { get; set; }
        public string MaxPoints { get; set; }
        public DateTime DeadLine { get; set; }

        [ForeignKey("Creator")]
        public string CreatorId { get; set; }

        public virtual ApplicationUser Creator { get; set; }
        public DateTime CreatedAt { get; set; }
        public virtual ICollection<Solving> Solvings { get; set; }
    }
}