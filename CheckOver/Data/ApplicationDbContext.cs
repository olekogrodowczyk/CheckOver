using CheckOver.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CheckOver.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Exercise> Exercises { get; set; }
        public DbSet<Invitation> invitations { get; set; }
#pragma warning disable CS0114 // Member hides inherited member; missing override keyword
        public DbSet<Role> Roles { get; set; }
#pragma warning restore CS0114 // Member hides inherited member; missing override keyword
        public DbSet<ExerciseState> ExerciseStates { get; set; }
        public DbSet<Assignment> Assignments { get; set; }
        public DbSet<Group> Groups { get; set; }
    }
}