using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using ModelWpf.Data;
using ModelWpf.DAL;
using Prism.Mvvm;

namespace ModelWpf
{


    class MainWindowViewModel : BindableBase
    {


        private Repository _repository;
        public MainWindowViewModel()
        {
            _repository = new Repository();
        }


        #region Properties

        public ObservableCollection<Model> Models
        {
            get
            {
                var models = _repository.GetAllModels().Result;
                return new ObservableCollection<Model>(models);
                //RaisePropertyChanged("Models");
            }
        }

        public ObservableCollection<Assignment> PlannedAss
        {
            get
            {
                var plannedAssList = _repository.GetPlannedAssignments().Result;
                return new ObservableCollection<Assignment>(plannedAssList);
                //RaisePropertyChanged("PlannedAss");
            }
        }

        public ObservableCollection<Assignment> IncomingAss
        {
            get
            {
                var incomingAssList = _repository.GetIncomingAssignments().Result;
                return new ObservableCollection<Assignment>(incomingAssList);
                //RaisePropertyChanged("IncomingAss");
            }
        }



        #endregion
    }
}
