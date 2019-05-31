using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ModelWpf.Data;
using Prism.Commands;
using Prism.Mvvm;

namespace ModelWpf
{
    class AddModelViewModel : BindableBase
    {

        private Model _newModel;
        public AddModelViewModel(Model newModel)
        {
            _newModel = newModel;
        }


        public Model NewModel
        {
            get { return _newModel;}
            set { SetProperty(ref _newModel, value); }
        }

    }
}
