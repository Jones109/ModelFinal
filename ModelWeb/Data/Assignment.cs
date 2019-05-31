using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ModelWeb.Data
{
    public class Assignment
    {
        public int Id { get; set; }
        [Required]
        public string Client { get; set; }
        [Required]
        public DateTime StartDate { get; set; }
        [Required]
        public int DurationDays { get; set; }
        [Required]
        public string Location { get; set; }
        [Required]
        public int NumModels { get; set; }
        public string Comments { get; set; }

        public List<Expense> Expenses { get; set; }



    }
}
