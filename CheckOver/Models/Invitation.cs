using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CheckOver.Models
{
    public class Invitation
    {
        [Key]
        public int InvitationId { get; set; }

        [ForeignKey("Sender")]
        public string SenderId { get; set; }

        public virtual ApplicationUser Sender { get; set; }

        [ForeignKey("Receiver")]
        public string ReceiverId { get; set; }

        public virtual ApplicationUser Receiver { get; set; }
        public string Status { get; set; }

        [ForeignKey("Role")]
        public int RoleId { get; set; }

        public virtual Role Role { get; set; }

        [ForeignKey("Group")]
        public int GroupId { get; set; }

        public virtual Group Group { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}