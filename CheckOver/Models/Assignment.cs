using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CheckOver.Models
{
    public class Assignment
    {
        public int Id { get; set; }
        public ICollection<ApplicationUser> ApplicationUsers { get; set; }
        public ICollection<State> States{ get; set; }
        public Invitation Invitation { get; set; }
    }
}
