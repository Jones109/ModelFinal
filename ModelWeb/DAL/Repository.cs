using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ModelWeb.Data;

namespace ModelWeb.DAL
{

    public interface IRepository
    {
      
    }

    public class Repository
    {

        private ApplicationDbContext _context;
        public Repository(ApplicationDbContext context)
        {
            _context = context;

        }
    }
}
