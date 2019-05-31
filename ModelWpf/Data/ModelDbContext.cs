using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ModelWpf.Data
{
    class ModelDbContext : DbContext
    {
        public DbSet<Model> Models { get; set; }
        public DbSet<Assignment> Assignments { get; set; }
        public DbSet<Model_Assignment> Model_Assignments { get; set; }

        public ModelDbContext()
        {
        }

        public ModelDbContext(DbContextOptions<ModelDbContext> options) : base(options)
        { }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=ModelDB;Trusted_Connection=true;MultipleActiveResultSets=true");

            }
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Model_Assignment>().HasKey(ma => new { ma.AssignmentId, ma.ModelId });


            modelBuilder.Entity<Model_Assignment>()
                .HasOne<Assignment>(ma => ma.Assignment)
                .WithMany(a => a.Model_Assignments)
                .HasForeignKey(ma => ma.AssignmentId);

            modelBuilder.Entity<Model_Assignment>()
                .HasOne<Model>(ma => ma.Model)
                .WithMany(m => m.Model_Assignments)
                .HasForeignKey(ma => ma.ModelId);
        }
    }
}
