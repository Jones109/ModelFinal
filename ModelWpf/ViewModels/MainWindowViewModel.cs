using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using DAL;
using ModelWpf.ViewModels;
using ModelWpf.Views;
using Prism.Commands;
using Prism.Mvvm;

namespace ModelWpf
{


    class MainWindowViewModel : BindableBase
    {

        AddModelWindow modelWindow = null;
        private Repository _repository;
        public MainWindowViewModel()
        {
            _repository = new Repository();
            _repository.CreateDB();


            /*
            int modelId = _repository.InsertModel(new Model
            {
                Address = "a", HairColor = "a", Height = 1, Weight = 1, Name = "a", PhoneNumber = "1234"
            });

            int assId = _repository.InsertAssignment(new Assignment{Client = "a", DurationDays = 1, Location = "a", NumModels = 2, Planned = false, StartDate = DateTime.Now});


            _repository.AddModelToAssignment(modelId, assId);
            */
        }


        #region Properties

        public ObservableCollection<Model> Models
        {
            get
            {
                var models = _repository.GetAllModels();
                return models.Result;
                //RaisePropertyChanged("Models");
            }
        }

        Model currentModel = null;

        public Model CurrentModel
        {
            get { return currentModel; }
            set
            {
                SetProperty(ref currentModel, value);
            }
        }

        int currentModelIndex = -1;
        public int CurrentModelIndex
        {
            get { return currentModelIndex; }
            set
            {
                SetProperty(ref currentModelIndex, value);
            }
        }

        public ObservableCollection<Assignment> PlannedAss
        {
            get
            {
                var plannedAssList = _repository.GetPlannedAssignments();
                return plannedAssList.Result;
                //RaisePropertyChanged("PlannedAss");
            }
        }
        Assignment currentAss = null;

        public Assignment CurrentAss
        {
            get { return currentAss; }
            set
            {
                SetProperty(ref currentAss, value);
            }
        }

        int currentPlannedAssIndex = -1;
        public int CurrentPlannedAssIndex
        {
            get { return currentPlannedAssIndex; }
            set
            {
                SetProperty(ref currentPlannedAssIndex, value);
            }
        }

        public ObservableCollection<Assignment> IncomingAss
        {
            get
            {
                var incomingAssList = _repository.GetIncomingAssignments();
                return incomingAssList.Result;
                //RaisePropertyChanged("IncomingAss");
            }
        }

       

        int currentIncomingAssIndex = -1;
        public int CurrentIncomingAssIndex
        {
            get { return currentIncomingAssIndex; }
            set
            {
                SetProperty(ref currentIncomingAssIndex, value);
            }
        }

        #endregion

        #region Commands

        ICommand _addNewModelCommand;

        public ICommand AddNewModelCommand
        {
            get
            {
                return _addNewModelCommand ?? (_addNewModelCommand = new DelegateCommand(() =>
                {
                    Model newModel = new Model();
                    var vm = new AddModelViewModel(newModel);
                    AddModelWindow amw = new AddModelWindow
                    {
                        DataContext = vm,
                        Owner = Application.Current.MainWindow.Owner
                    };
                    if (amw.ShowDialog() == true)
                    {
                        _repository.InsertModel(newModel);
                        RaisePropertyChanged("Models");
                    }
                }));
            }
        }


        ICommand _addNewAssignmentCommand;

        public ICommand AddNewAssignmentCommand
        {
            get
            {
                return _addNewAssignmentCommand ?? (_addNewModelCommand = new DelegateCommand(() =>
                {
                    Assignment newAss = new Assignment();
                    var vm = new AddAssignmentViewModel(newAss);
                    AddAssignmentWindow amw = new AddAssignmentWindow()
                    {
                        DataContext = vm,
                        Owner = Application.Current.MainWindow.Owner
                    };
                    if (amw.ShowDialog() == true)
                    {
                        _repository.InsertAssignment(newAss);
                        RaisePropertyChanged("IncomingAss");
                    }
                }));
            }
        }

        private ICommand _addModelToAssignmentCommand;

        public ICommand AddModelToAssignmentCommand
        {
            get { return _addNewAssignmentCommand ?? (_addModelToAssignmentCommand = new DelegateCommand(() =>
            {
                //add model to assignment
                var assignments = _repository.AddModelToAssignment(CurrentModel.Id, CurrentAss.Id);

                //Check if Assignment is full
                var ass = assignments.Single();
                if (ass.Model_Assignments.Count >= ass.NumModels)
                {
                    ass.Planned = true;
                    _repository.UpdateAssignment(ass);
                }

                RaisePropertyChanged("IncomingAss");
                RaisePropertyChanged("PlannedAss");
            })); }
        }


        #endregion

    }
}
