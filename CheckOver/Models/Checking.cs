using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CheckOver.Models
{
    public class Checking
    {
        [Key]
        public int CheckingId { get; set; }

        [ForeignKey("Checker")]
        public string CheckerId { get; set; }

        public virtual ApplicationUser Checker { get; set; }
        public string Remarks { get; set; }
        public int Points { get; set; }
        public DateTime CreatedAt { get; set; }
        public virtual Solving Solving { get; set; }
    }
}