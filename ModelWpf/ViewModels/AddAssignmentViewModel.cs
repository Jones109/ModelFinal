using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelWpf.Data;
using Prism.Mvvm;

namespace ModelWpf.ViewModels
{
    class AddAssignmentViewModel:BindableBase
    {
        private Assignment _assignment;

        public AddAssignmentViewModel(Assignment assignment)
        {
            _assignment = assignment;
        }

        public Assignment Assignment {
            get { return _assignment; }
            set { SetProperty(ref _assignment, value); }
        }
    }
}
