using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelFinal.Data
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
        [Required]
        public bool Planned { get; set; }

        //Many to many with Models
        public List<Model_Assignment> Model_Assignments { get; set; }
    }
}
