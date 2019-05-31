using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ModelWeb.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            DbSet<Assignment> Assignments;
            DbSet<Expense> Expenses;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
            modelBuilder.Entity<Assignment>()
                .HasMany(a => a.Expenses)
                .WithOne(e => e.Assignment)
                .HasForeignKey(e => e.AssignmentId);
        }
    }
}
