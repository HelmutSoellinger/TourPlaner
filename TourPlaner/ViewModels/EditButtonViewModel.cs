using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using TourPlaner.Models;

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
                   // AddNewTourCommand.RaiseCanExecuteChanged(); // inform the [Add] Button, that the enabled/disabled state should update
                }
            }

            public string NewTourName
        {
                get => newTourName;
                set
            {
                    newTourName = value;
                    OnPropertyChanged(nameof(NewTourName));
                    AddNewTourCommand.RaiseCanExecuteChanged(); // inform the [Add] Button, that the enabled/disabled state should update
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
                    DeleteTourCommand.RaiseCanExecuteChanged(); // inform the [Delete] Button, that the enabled/disabled state should update
                }
            }

            public TourModel? SelectedTour
            {
                    get => selectedTour;
                    set
                {
                        selectedTour = value;
                        DeleteTourCommand.RaiseCanExecuteChanged(); // inform the [Delete] Button, that the enabled/disabled state should update
                    }
                }


            public RelayCommand AddNewTourCommand { get; }
            public RelayCommand DeleteTourCommand { get; }
            
            public RelayCommand AddNewLogCommand { get; }
            public RelayCommand DeleteLogCommand { get; }


            public event EventHandler<LogModel>? AddButtonClicked;

            public event EventHandler<LogModel?>? DeleteButtonClicked;



            public EditButtonViewModel()
            {
                AddNewTourCommand = new RelayCommand((_) =>
                {
                    AddButtonClicked?.Invoke(this, new LogModel() { Date = DateTime.Now });
                }, (_) => !string.IsNullOrWhiteSpace(NewLogName));


                DeleteTourCommand = new RelayCommand((_) =>
                {
                    DeleteButtonClicked?.Invoke(this, SelectedLog);
                }, (_) => SelectedLog != null);
            }
        }
    }

