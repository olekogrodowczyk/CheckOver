using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CheckOver.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime CreatedAt { get; set; }

        [InverseProperty("Sender")]
        public virtual ICollection<Invitation> InvitationsSent { get; set; }

        [InverseProperty("Receiver")]
        public virtual ICollection<Invitation> InvitationsReceived { get; set; }

        [InverseProperty("User")]
        public virtual ICollection<Assignment> Assignments { get; set; }

        [InverseProperty("Creator")]
        public virtual ICollection<Exercise> ExercisesCreated { get; set; }

        [InverseProperty("Checker")]
        public virtual ICollection<Checking> Checkings { get; set; }

        [InverseProperty("Creator")]
        public virtual ICollection<Group> GroupsCreated { get; set; }
    }
}