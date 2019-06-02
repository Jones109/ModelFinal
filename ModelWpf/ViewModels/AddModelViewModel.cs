using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using DAL;
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


        public event EventHandler Apply;
        private ICommand _acceptCommand;
        public ICommand AcceptCommand {
            get
            {
                return _acceptCommand ?? (_acceptCommand = new DelegateCommand(() =>
                           {
                               if (Apply!=null)
                               {
                                   Apply(this, EventArgs.Empty);
                               }
                               
                           }));
            }
        }


        public event EventHandler Close;
        private ICommand _closeCommand;
        public ICommand CloseCommand
        {
            get
            {
                return _closeCommand ?? (_closeCommand = new DelegateCommand(() =>
                {
                    if (Close != null)
                    {
                        Close(this, EventArgs.Empty);
                    }

                }));
            }
        }



    }
}
