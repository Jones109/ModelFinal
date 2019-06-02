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
        private AddModelWindow amw = null;
        private AddModelViewModel vm = null;

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
                    if (amw != null)
                    {
                        amw.Focus();
                    }
                    else
                    {
                        Model newModel = new Model();
                        vm = new AddModelViewModel(newModel);
                        amw = new AddModelWindow
                        {
                            DataContext = vm,
                            Owner = Application.Current.MainWindow.Owner
                        };

                        vm.Apply += new EventHandler(addModelApply);
                        vm.Close += new EventHandler(addModelClose);
                        amw.Closed += new EventHandler(addModelClose);
                        amw.Show();


                        
                    }

                    
                }));
            }
        }

        private void addModelApply(object sender, EventArgs e)
        {
  
            _repository.InsertModel(vm.NewModel);
            RaisePropertyChanged("Models");
            amw.Close();

        }

        private void addModelClose(object sender, EventArgs e)
        {
            vm.Apply -= new EventHandler(addModelApply);
            vm.Close -= new EventHandler(addModelClose);
            amw.Closed -= new EventHandler(addModelClose);
            amw = null;
            vm = null;
            Application.Current.MainWindow.Focus();
        }

        private ICommand _deleteModelCommand;

        public ICommand DeleteModelCommand
        {
            get
            {
                return _deleteModelCommand ?? (_deleteModelCommand = new DelegateCommand(() =>
                {
                    MessageBoxResult result = MessageBox.Show($"Vil du slette Model: {CurrentModel.Id}", "Er du sikker?", MessageBoxButton.YesNo);
                    switch (result)
                    {
                        case MessageBoxResult.Yes:

                            MessageBox.Show($"Model: {CurrentModel.Id} er slettet", "Slettet");
                            _repository.DeleteModel(CurrentModel);
                            RaisePropertyChanged("Models");
                            RaisePropertyChanged("IncomingAss");
                            RaisePropertyChanged("PlannedAss");

                            break;
                        case MessageBoxResult.No:

                            break;
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

        private ICommand _deleteAssignmentCommand;

        public ICommand DeleteAssignmentCommand
        {
            get
            {
                return _deleteAssignmentCommand ?? (_deleteAssignmentCommand = new DelegateCommand(() =>
                           {
                              

                               MessageBoxResult result = MessageBox.Show($"Vil du slette Opgave: {CurrentAss.Id}", "Er du sikker?", MessageBoxButton.YesNo);
                               switch (result)
                               {
                                   case MessageBoxResult.Yes:
                                       
                                       MessageBox.Show($"Opgave: {CurrentAss.Id} er slettet", "Slettet");
                                       _repository.DeleteAssignment(CurrentAss);
                                       RaisePropertyChanged("IncomingAss");
                                       RaisePropertyChanged("PlannedAss");

                                       break;
                                   case MessageBoxResult.No:

                                       break;
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


        private ICommand _removeModelFromAssignmentCommand;

        public ICommand RemoveModelFromAssignmentCommand
        {
            get
            {
                return _removeModelFromAssignmentCommand ?? (_removeModelFromAssignmentCommand = new DelegateCommand(
                           () =>
                           {
                               MessageBoxResult result = MessageBox.Show($"Vil du fjerne Model: {CurrentModel.Id} fra Opgave: {CurrentAss.Id}", "Er du sikker?", MessageBoxButton.YesNo);
                               switch (result)
                               {
                                   case MessageBoxResult.Yes:
                                       _repository.RemoveModelFromAssignment(CurrentModel.Id, CurrentAss.Id);
                                       MessageBox.Show($"Model: {CurrentModel.Id} er fjernet fra Opgave: {CurrentAss.Id}", "Fjernet");
                                       RaisePropertyChanged("Models");
                                       RaisePropertyChanged("IncomingAss");
                                       RaisePropertyChanged("PlannedAss");
                                       
                                       break;
                                   case MessageBoxResult.No:
                                       
                                       break;
                               }

                               
                           }));
            }
        }

        #endregion

    }
}
