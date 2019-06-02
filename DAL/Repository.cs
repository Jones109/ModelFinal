using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

namespace DAL
{


    public interface IRepository
    {
        Task<ObservableCollection<Model>> GetAllModels();
        int InsertModel(Model model);
        int InsertAssignment(Assignment assignment);
        Task<ObservableCollection<Assignment>> GetPlannedAssignments();
        Task<ObservableCollection<Assignment>> GetIncomingAssignments();

        List<Assignment> AddModelToAssignment(int modelId, int assignmentId);
        void DeleteModel(Model model);
        void DeleteAssignment(Assignment assignment);
        bool CreateDB();
    }

    public class Repository
    {
        private DbContextOptions<AppDbContext> _options;
        public Repository()
        {
            _options = new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=ModelDBSamlet;Trusted_Connection=true;MultipleActiveResultSets=true")
                           .Options;
        }

        public bool CreateDB()
        {
            using (var context = new AppDbContext(_options))
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
            using (var context = new AppDbContext(_options))
            {
                return new ObservableCollection<Model>(await context.Models.ToListAsync());
            }
        }

        public async Task<ObservableCollection<Model>> GetAllModelsNoLogin()
        {
            using (var context = new AppDbContext(_options))
            {
                return new ObservableCollection<Model>(await context.Models.Where(x=>x.AppUser==null).ToListAsync());
            }
        }

        public int InsertModel(Model model)
        {
            using (var context = new AppDbContext(_options))
            {
                context.Models.Add(model);
                context.SaveChanges();
                return model.Id;
            }
        }

        public void DeleteModel(Model model)
        {
            using (var context = new AppDbContext(_options))
            {
                var modelAssignments = context.Model_Assignments.Where(x => x.ModelId == model.Id).ToList();

                var assignments = context.Assignments.Where(z => modelAssignments.Exists(x => z.Id == x.AssignmentId)).Include(a => a.Model_Assignments)
                    .ToList();

                context.Models.Remove(model);
                context.SaveChanges();

                List<Assignment> updatedAssignments = new List<Assignment>();
                foreach (var ass in assignments)
                {
                    updatedAssignments.AddRange(context.Assignments.Where(x => x.Id == ass.Id).Include(z => z.Model_Assignments).ToList()); 
                }

                foreach (var ass in updatedAssignments)
                {
                    if (ass.Model_Assignments.Count < ass.NumModels)
                    {
                        ass.Planned = false;
                    }
                }

                context.UpdateRange(updatedAssignments);
                context.SaveChanges();
            }
        }

 

        #endregion


        #region Assignment

        public List<Assignment> GetPreviousAss(int modelId)
        {
            using (var context = new AppDbContext(_options))
            {
                List<Assignment> assignments = context.Assignments.Where(x => x.StartDate.AddDays(x.DurationDays) < DateTime.Now)
                    .Where(x => x.Model_Assignments.Exists(z => z.ModelId == modelId)).ToList();

                return assignments;
            }
        }

        public List<Assignment> GetLaterAss(int modelId)
        {
            using (var context = new AppDbContext(_options))
            {
                List<Assignment> assignments = context.Assignments.Where(x => x.StartDate.AddDays(x.DurationDays) > DateTime.Now)
                    .Where(x => x.Model_Assignments.Exists(z => z.ModelId == modelId)).ToList();

                return assignments;
            }
        }



        public int InsertAssignment(Assignment assignment)
        {
            using (var context = new AppDbContext(_options))
            {
                context.Assignments.Add(assignment);
                context.SaveChanges();
                return assignment.Id;
            }
        }

        public void DeleteAssignment(Assignment assignment)
        {
            using (var context = new AppDbContext(_options))
            {
                context.Assignments.Remove(assignment);
                context.SaveChanges();
                
            }
        }

        public async Task<ObservableCollection<Assignment>> GetPlannedAssignments()
        {
            using (var context = new AppDbContext(_options))
            {
                return new ObservableCollection<Assignment>(await context.Assignments.Where(x => x.Planned == true).ToListAsync());
            }
        }

        public async Task<ObservableCollection<Assignment>> GetIncomingAssignments()
        {
            using (var context = new AppDbContext(_options))
            {
                return new ObservableCollection<Assignment>(await context.Assignments.Where(x => x.Planned == false).ToListAsync());
            }
        }

        public void UpdateAssignment(Assignment ass)
        {
            using (var context = new AppDbContext(_options))
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
            using (var context = new AppDbContext(_options))
            {


                Model_Assignment newModel_Assignment = new Model_Assignment
                {
                    AssignmentId = assignmentId,
                    ModelId = modelId
                };

                if (!context.Model_Assignments.Any(x => x.AssignmentId == assignmentId && x.ModelId == modelId))
                {
                    context.Model_Assignments.Add(newModel_Assignment);
                    context.SaveChanges();
                }

                

                return context.Assignments.Where(x => x.Id == assignmentId).Include(x => x.Model_Assignments).ToList();

            }
        }


        public void RemoveModelFromAssignment(int modelId, int assignmentId)
        {
            using (var context = new AppDbContext(_options))
            {


                Model_Assignment deleteModel_Assignment = new Model_Assignment
                {
                    AssignmentId = assignmentId,
                    ModelId = modelId
                };

                context.Model_Assignments.Remove(deleteModel_Assignment);
                context.SaveChanges();

                var ass = context.Assignments.Where(x => x.Id == assignmentId).Include(z=>z.Model_Assignments).ToList().First();

                if (ass.Model_Assignments.Count < ass.NumModels)
                {
                    ass.Planned = false;
                }

                context.Assignments.Update(ass);
                context.SaveChanges();
            }
        }


        #endregion

        #region Expense

        public void InsertExpense(Expense expense)
        {
            using (var context = new AppDbContext(_options))
            {
                context.Expenses.Add(expense);
                context.SaveChanges();
            }
        }

        public List<Expense> GetExpensesForAss(int assId)
        {
            using (var context = new AppDbContext(_options))
            {
                var expenses = context.Expenses.Where(x => x.AssignmentId == assId).ToList();
                return expenses;
            }
            
        }



        #endregion

    }
}
