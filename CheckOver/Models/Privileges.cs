using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CheckOver.Models
{
    public class Privileges
    {
        public int Id { get; set; }
        public Role Role { get; set; }
        public string Description { get; set; }
    }
}
