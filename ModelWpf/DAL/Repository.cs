using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
                .UseSqlServer("Server = (localdb)\\MSSQLLocalDB; Database = ModelDB; Trusted_Connection = true; MultipleActiveResultSets = true")
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


        public async Task<ObservableCollection<Model>> GetAllModels()
        {
            using (var context = new ModelDbContext(_options))
            {
                return new ObservableCollection<Model>(await context.Models.ToListAsync());
            }
        }

        public int InsertModel(Model model)
        {
            using (var context = new ModelDbContext(_options))
            {
                 context.Models.Add(model);
                 context.SaveChanges();
                return model.Id;
            }
        }


        #endregion


        #region Assignment


        public int InsertAssignment(Assignment assignment)
        {
            using (var context = new ModelDbContext(_options))
            {
                context.Assignments.Add(assignment);
                context.SaveChanges();
                return assignment.Id;
            }
        }

        public async Task<ObservableCollection<Assignment>> GetPlannedAssignments()
        {
            using (var context = new ModelDbContext(_options))
            {
                return new ObservableCollection<Assignment>( await context.Assignments.Where(x => x.Planned == true).ToListAsync());
            }
        }

        public async Task<ObservableCollection<Assignment>> GetIncomingAssignments()
        {
            using (var context = new ModelDbContext(_options))
            {
                return new ObservableCollection<Assignment>( await context.Assignments.Where(x => x.Planned == false).ToListAsync());
            }
        }

        public void UpdateAssignment(Assignment ass)
        {
            using (var context = new ModelDbContext(_options))
            {
                var entity = context.Assignments.FirstOrDefault(
                    h => h.Id == ass.Id);

                entity.Planned = ass.Planned;

                context.Update(entity);

                context.SaveChanges();
            }

        }
        #endregion


        #region Model_Assignment

        public List<Assignment> AddModelToAssignment(int modelId, int assignmentId)
        {
            using (var context = new ModelDbContext(_options))
            {
                Model_Assignment newModel_Assignment = new Model_Assignment
                {
                    AssignmentId = assignmentId,
                    ModelId = modelId
                };
               

                context.Model_Assignments.Add(newModel_Assignment);
                context.SaveChanges();

                return context.Assignments.Where(x => x.Id == assignmentId).Include(x=>x.Model_Assignments).ToList();

            }
        }

        #endregion

    }
}
