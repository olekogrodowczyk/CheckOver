﻿using CheckOver.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CheckOver.Repository
{
    public class PrivilegeRepository : IPrivilegeRepository
    {
        private readonly ApplicationDbContext _context;

        public PrivilegeRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public int function()
        {
            return 0;
        }
    }
}