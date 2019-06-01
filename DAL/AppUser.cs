using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace DAL
{
    public class AppUser:IdentityUser
    {
        public int ModelId { get; set; }
        public Model Model { get; set; }
    }
}
