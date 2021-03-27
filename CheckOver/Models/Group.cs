using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CheckOver.Models
{
    public class Group
    {
        public int Id { get; set; }
        public DateTime? CreateDate{ get; set; }
        public ICollection<Invitation> Invitations{ get; set; }
        public ApplicationUser Admin { get; set; }
    }
}
