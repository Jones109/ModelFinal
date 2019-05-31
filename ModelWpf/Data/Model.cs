using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelWpf.Data
{
    public class Model
    {

        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public int Height { get; set; }
        [Required]
        public int Weight { get; set; }
        [Required]
        public string HairColor { get; set; }

        public string Comments { get; set; }

        //Many to many with Models
        public List<Model_Assignment> Model_Assignments { get; set; }
    }
}
