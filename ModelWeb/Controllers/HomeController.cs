using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using DAL;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using ModelWeb.Models;

namespace ModelWeb.Controllers
{
    public class HomeController : Controller
    {
        private UserManager<AppUser> _userManager;
        private SignInManager<AppUser> _signInManager;
        private Repository _repository;


        public HomeController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _repository = new Repository();
            _repository.CreateDB();
        }
       
        public IActionResult Index()
        {
            if (_signInManager.IsSignedIn(User))
            {
                return RedirectToAction("Assignments");
            }
            return View(_repository.GetAllModelsNoLogin().Result);
        }



        public IActionResult Assignments()
        {
            AppUser currentUser = _userManager.FindByNameAsync(User.Identity.Name).Result;

            List<List<Assignment>> list = new List<List<Assignment>>();
            list.Add(_repository.GetPreviousAss(currentUser.ModelId));
            list.Add(_repository.GetLaterAss(currentUser.ModelId));

            return View(list);

        }

        public IActionResult AddExpense(int id)
        {

            Expense exp = new Expense
            {
                AssignmentId = id
            };
            return View(exp);
        }

        public IActionResult AddExpenseSubmit(Expense expense)
        {
            Expense newExpense = new Expense
            {
                Amount = expense.Amount,
                Date = expense.Date,
                Text = expense.Text,
                AssignmentId = expense.AssignmentId
            };
            
            _repository.InsertExpense(newExpense);
            return RedirectToAction("Index");
        }

        public IActionResult SeeExpenses(int id)
        {
            return View(_repository.GetExpensesForAss(id));
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
