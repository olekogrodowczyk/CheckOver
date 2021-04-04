using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CheckOver.Models
{
    public class Group
    {
        public int Id { get; set; }
        public DateTime? CreateDate { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Invitation> Invitations { get; set; }
        public virtual ICollection<Assignment> Assignments { get; set; }
        public virtual ApplicationUser Admin { get; set; }

        [Display(Name = "Wybierz obrazek swojej grupy")]
        public string CoverImageUrl { get; set; }
    }
}