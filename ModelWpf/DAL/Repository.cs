using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using ModelWpf.Data;

namespace ModelWpf.DAL
{
    public class Repository
    {
        private DbContextOptions<ModelDbContext> _options;
        public Repository()
        {
            _options = new DbContextOptionsBuilder<ModelDbContext>()
                .UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=ModelDB;Trusted_Connection=true;MultipleActiveResultSets=true")
                .Options;
        }

        public bool CreateDB()
        {
            using (var context = new ModelDbContext(_options))
            {
                if (true && (context.GetService<IDatabaseCreator>() as RelationalDatabaseCreator).Exists())
                    return false;

                context.Database.EnsureDeleted();
                return context.Database.EnsureCreated();
            }
        }

        #region Models


        public async Task<List<Model>> GetAllModels()
        {
            using (var context = new ModelDbContext(_options))
            {
                return await context.Models.ToListAsync();
            }
        }

        public async Task InsertModel(Model model)
        {
            using (var context = new ModelDbContext(_options))
            {
                await context.Models.AddAsync(model);
                await context.SaveChangesAsync();
            }
        }


        #endregion


        #region Assignment


        public async Task InsertAssignment(Assignment assignment)
        {
            using (var context = new ModelDbContext(_options))
            {
                await context.Assignments.AddAsync(assignment);
                await context.SaveChangesAsync();
            }
        }

        public async Task<List<Assignment>> GetPlannedAssignments()
        {
            using (var context = new ModelDbContext(_options))
            {
                return await context.Assignments.Where(x => x.Planned == true).ToListAsync();
            }
        }

        public async Task<List<Assignment>> GetIncomingAssignments()
        {
            using (var context = new ModelDbContext(_options))
            {
                return await context.Assignments.Where(x => x.Planned == false).ToListAsync();
            }
        }

        #endregion


        #region Model_Assignment

        public async Task AddModelToAssignment(int modelId, int assignmentId)
        {
            using (var context = new ModelDbContext(_options))
            {
                Model_Assignment newModel_Assignment = new Model_Assignment
                {
                    AssignmentId = assignmentId,
                    ModelId = modelId
                };

                await context.Model_Assignments.AddAsync(newModel_Assignment);
                await context.SaveChangesAsync();



            }
        }

        #endregion

    }
}
