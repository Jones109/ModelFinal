using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class Model
    {

        public int Id { get; set; }

        [Required]
        [DisplayName("Navn")]
        public string Name { get; set; }
        [Required]
        [DisplayName("Telefonnummer")]
        public string PhoneNumber { get; set; }
        [Required]
        [DisplayName("Adresse")]
        public string Address { get; set; }
        [Required]
        [DisplayName("Højde")]
        public int Height { get; set; }
        [Required]
        [DisplayName("Vægt")]
        public int Weight { get; set; }
        [Required]
        [DisplayName("Hårfarve")]
        public string HairColor { get; set; }

        [DisplayName("Bemærkning")]
        public string Comments { get; set; }

        //Many to many with Models
        public List<Model_Assignment> Model_Assignments { get; set; }

        public AppUser AppUser { get; set; }
    }
}
