﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CheckOver.Models
{
    public class Role
    {
        public int Id { get; set; }
        public ICollection<Invitation> Invitations { get; set; }
        public string Description { get; set; }
    }
}
