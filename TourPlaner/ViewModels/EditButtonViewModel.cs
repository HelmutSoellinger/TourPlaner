using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using TourPlaner.Models;
using TourPlaner.Views;

namespace TourPlaner.ViewModels
{
   
        public class EditButtonViewModel : BaseViewModel
        {
            private string newLogName = "New log";
            private string newTourName = "New tour";

            public string NewLogName
            {
                get => newLogName;
                set
                {
                    newLogName = value;
                    OnPropertyChanged(nameof(NewLogName));
                    AddNewTourCommand.RaiseCanExecuteChanged(); // inform the [Add] Button, that the enabled/disabled state should update 
                   //outcommented so the add button stays enabled
                }
            }

            public string NewTourName
        {
                get => newTourName;
                set
            {
                    newTourName = value;
                    OnPropertyChanged(nameof(NewTourName));
                   // AddNewTourCommand.RaiseCanExecuteChanged(); // inform the [Add] Button, that the enabled/disabled state should update
                }
            }

            private LogModel? selectedLog;
            private TourModel? selectedTour;

            public LogModel? SelectedLog
            {
            get => selectedLog;
            set
            {
                selectedLog = value;
                DeleteLogCommand.RaiseCanExecuteChanged(); // inform the [Delete] Button, that the enabled/disabled state should update
                ModifyLogCommand.RaiseCanExecuteChanged();
            }
        }
        public TourModel? SelectedTour
        {
            get => selectedTour;
            set
            {
                selectedTour = value;
                AddNewLogCommand.RaiseCanExecuteChanged(); // inform the [Add] Log button to enable/disable
                ModifyTourCommand.RaiseCanExecuteChanged();
                DeleteTourCommand.RaiseCanExecuteChanged();// inform the [Delete] Button, that the enabled/disabled state should update
                ModifyLogCommand.RaiseCanExecuteChanged();
            }
        }



        public RelayCommand AddNewLogCommand { get; }
        public RelayCommand DeleteLogCommand { get; }
        public RelayCommand ModifyLogCommand { get; }
        public RelayCommand AddNewTourCommand { get; }
        public RelayCommand DeleteTourCommand { get; }
        public RelayCommand ModifyTourCommand { get; }


        public event EventHandler<LogModel>? AddLogButtonClicked;
        public event EventHandler<LogModel?>? DeleteLogButtonClicked;
        public event EventHandler<LogModel?>? ModifyLogButtonClicked;
        public event EventHandler<TourModel>? AddTourButtonClicked;
        public event EventHandler<TourModel?>? DeleteTourButtonClicked;
        public event EventHandler<TourModel?>? ModifyTourButtonClicked;

        public EditButtonViewModel()
        {
            AddNewLogCommand = new RelayCommand((_) =>
            {
                NewLogPopupViewModel viewModel = new NewLogPopupViewModel();
                NewLogPopup popup = new NewLogPopup { DataContext= viewModel };
                popup.ShowDialog();
                if(viewModel.Result)
                    AddLogButtonClicked?.Invoke(this, viewModel.LogModel);
            }, (_) => SelectedTour != null); //not clickable when no tour is selected

            DeleteLogCommand = new RelayCommand((_) =>
            {
                DeleteLogButtonClicked?.Invoke(this, SelectedLog);
            }, (_) => SelectedLog != null); //not clickable when no log is selected

            ModifyLogCommand = new RelayCommand((_) =>
            {
                NewLogPopupViewModel viewModel = new NewLogPopupViewModel(SelectedLog);
                NewLogPopup popup = new NewLogPopup { DataContext = viewModel };
                popup.ShowDialog();
                if (viewModel.Result)
                    ModifyLogButtonClicked?.Invoke(this, viewModel.LogModel);
            }, (_) => SelectedLog != null); //not clickable when no log is selected

            AddNewTourCommand = new RelayCommand((_) =>
            {
                NewTourPopupViewModel viewModel = new NewTourPopupViewModel();
                NewTourPopup popup = new NewTourPopup { DataContext = viewModel };
                popup.ShowDialog();
                if (viewModel.Result)
                    AddTourButtonClicked?.Invoke(this, viewModel.TourModel);
            }, (_) => true);//new tour is always clickable

            DeleteTourCommand = new RelayCommand((_) =>
            {
                DeleteTourButtonClicked?.Invoke(this, SelectedTour);
            }, (_) => SelectedTour != null); //not clickable when no tour is selected

            ModifyTourCommand = new RelayCommand((_) =>
            {
                NewTourPopupViewModel viewModel = new NewTourPopupViewModel(SelectedTour);
                NewTourPopup popup = new NewTourPopup { DataContext = viewModel };
                popup.ShowDialog();
                if (viewModel.Result)
                    ModifyTourButtonClicked?.Invoke(this, viewModel.TourModel);
            }, (_) => SelectedTour != null); //not clickable when no tour is selected
        }
        public void EventTriggerUnitTest(object _model, string eventName)
        {
            switch (eventName) {
                case "AddLogButtonClicked":
                    {
                        if (_model is LogModel model)
                            AddLogButtonClicked?.Invoke(this, model);
                        break;
                    }
                    
                case "DeleteLogButtonClicked":
                    {
                        if (_model is LogModel model)
                            DeleteLogButtonClicked?.Invoke(this, model);
                        break;
                    }

                case "ModifyLogButtonClicked":
                    {
                        if (_model is LogModel model)
                            ModifyLogButtonClicked?.Invoke(this, model);
                        break;
                    }

                case "AddTourButtonClicked":
                    {
                        if (_model is TourModel model)
                            AddTourButtonClicked?.Invoke(this, model);
                        break;
                    }
;
                case "DeleteTourButtonClicked":
                    {
                        if (_model is TourModel model)
                            DeleteTourButtonClicked?.Invoke(this, model);
                        break;
                    }
                case "ModifyTourButtonClicked":
                    {
                        if (_model is TourModel model)
                            ModifyTourButtonClicked?.Invoke(this, model);
                        break;
                    }
            }
        }
    }
}

