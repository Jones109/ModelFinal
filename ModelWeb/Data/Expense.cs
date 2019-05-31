﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ModelWeb.Data
{
    public class Expense
    {
        public int Id { get; set; }

        [Required]
        public DateTime Date { get; set; }
        
        public string Text { get; set; }
        [Required]
        public double Amount { get; set; }

        [Required]
        public int AssignmentId { get; set; }

        public Assignment Assignment { get; set; }

    }
}
