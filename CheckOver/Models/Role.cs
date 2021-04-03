using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CheckOver.Models
{
    public class Role
    {
        public int Id { get; set; }
        public virtual ICollection<Invitation> Invitations { get; set; }
        public virtual ICollection<Assignment> Assignments { get; set; }
        public virtual ICollection<Privileges> Privileges { get; set; }
        public string Name { get; set; }
    }
}
