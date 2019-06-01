using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using DAL;
using ModelWeb.Models;

namespace ModelWeb.Controllers
{
    public class HomeController : Controller
    {
        private UserManager<AppUser> _userManager;
        private Repository _repository;


        public HomeController(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
            _repository = new Repository();
            _repository.CreateDB();
        }
       
        public IActionResult Index()
        {
            return View(_repository.GetAllModels().Result);
        }

        public IActionResult PreviousAss()
        {
            AppUser currentUser = _userManager.FindByNameAsync(User.Identity.Name).Result;

            return View(_repository.GetPreviousAss(currentUser.ModelId));
        }

        public IActionResult FutureAss()
        {
            AppUser currentUser = _userManager.FindByNameAsync(User.Identity.Name).Result;

            return View(_repository.GetLaterAss(currentUser.ModelId));
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
            return View("Index");
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
