using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CheckOver.Models
{
    public class Group
    {
        [Key]
        public int GroupId { get; set; }

        [ForeignKey("Creator")]
        public string CreatorId { get; set; }

        public virtual ApplicationUser Creator { get; set; }
        public string Name { get; set; }
        public DateTime CreatedAt { get; set; }

        public virtual ICollection<Invitation> Invitations { get; set; }
        public virtual ICollection<Assignment> Assignments { get; set; }

        [Display(Name = "Wybierz obrazek swojej grupy")]
        public string CoverImageUrl { get; set; }
    }
}