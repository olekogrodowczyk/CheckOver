﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CheckOver.Models
{
    public class Exercise
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int MaxPoints { get; set; }
        public DateTime? DeadLine { get; set; }
        public ExerciseState ExerciseState { get; set; }
        public string ProgrammingLanguage { get; set; }
    }
}