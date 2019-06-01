using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;


namespace DAL
{
    public class Assignment
    {
        public Assignment()
        {
            StartDate = DateTime.Now;
        }

        public int Id { get; set; }
        [Required]
        [DisplayName("Klient")]
        public string Client { get; set; }
        [Required]
        [DisplayName("Start dato")]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }
        [Required]
        [DisplayName("Varighed")]
        public int DurationDays { get; set; }
        [Required]
        [DisplayName("Sted")]
        public string Location { get; set; }
        [Required]
        [DisplayName("Antal modeller")]
        public int NumModels { get; set; }
        [DisplayName("Bemærkning")]
        public string Comments { get; set; }

        public List<Expense> Expenses { get; set; }

        public bool Planned { get; set; }


        public List<Model_Assignment> Model_Assignments { get; set; }



    }
}
   

