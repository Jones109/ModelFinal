using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DAL
{
    public class Expense
    {
        public Expense() {
            Date = DateTime.Now;

        }

        public int Id { get; set; }

        [Required]
        [DisplayName("Dato")]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }
        [DisplayName("Tekst")]
        public string Text { get; set; }
        [Required]
        [DisplayName("Beløb")]
        public double Amount { get; set; }

        [Required]
        public int AssignmentId { get; set; }

        public Assignment Assignment { get; set; }

    }
}
