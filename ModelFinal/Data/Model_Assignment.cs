using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelFinal.Data
{
    public class Model_Assignment
    {
        [Required]
        public int ModelId { get; set; }
        [Required]
        public Model Model { get; set; }
        [Required]
        public int AssignmentId { get; set; }

        [Required]
        public Assignment Assignment { get; set; }

    }
}
