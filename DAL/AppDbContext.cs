using System;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DAL
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        public DbSet<Assignment> Assignments { get; set; }
        public DbSet<Expense> Expenses { get; set; }
        public DbSet<Model> Models { get; set; }
        public DbSet<Model_Assignment> Model_Assignments { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=ModelDBSamlet;Trusted_Connection=true;MultipleActiveResultSets=true");

            }
        }

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {


        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Model_Assignment>().HasKey(ma => new { ma.AssignmentId, ma.ModelId });


            modelBuilder.Entity<Model_Assignment>()
                .HasOne<Assignment>(ma => ma.Assignment)
                .WithMany(a => a.Model_Assignments)
                .HasForeignKey(ma => ma.AssignmentId);

            modelBuilder.Entity<Model_Assignment>()
                .HasOne<Model>(ma => ma.Model)
                .WithMany(m => m.Model_Assignments)
                .HasForeignKey(ma => ma.ModelId);

            modelBuilder.Entity<Expense>()
                .HasOne<Assignment>(sc => sc.Assignment)
                .WithMany(s => s.Expenses)
                .HasForeignKey(sc => sc.AssignmentId);


            modelBuilder.Entity<Model>()
                .HasOne(s => s.AppUser)
                .WithOne(x => x.Model)
                .HasForeignKey<AppUser>();
        }
    }
}
