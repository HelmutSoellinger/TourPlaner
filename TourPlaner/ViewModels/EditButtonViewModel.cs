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


            public event EventHandler<LogModel>? AddLogButtonClicked;
            public event EventHandler<LogModel?>? DeleteLogButtonClicked;
            public event EventHandler<TourModel>? AddTourButtonClicked;
            public event EventHandler<TourModel?>? DeleteTourButtonClicked;



        public EditButtonViewModel()
            {
                AddNewLogCommand = new RelayCommand((_) =>
                {
                    AddLogButtonClicked?.Invoke(this, new LogModel() { Date = DateTime.Now });
                }, (_) => true);//!string.IsNullOrWhiteSpace(NewLogName));  outcommented so the add button stays functional



                DeleteLogCommand = new RelayCommand((_) =>
                        {
                                DeleteLogButtonClicked?.Invoke(this, SelectedLog);
                        }, (_) => SelectedLog != null);

                AddNewTourCommand = new RelayCommand((_) =>
                {
                    AddTourButtonClicked?.Invoke(this, new TourModel() { Name = NewTourName });
                    NewTourName = "New tour"; // Reset the tour name
                }, (_) => true);//!string.IsNullOrWhiteSpace(NewLogName));  outcommented so the add button stays functional



                DeleteTourCommand = new RelayCommand((_) =>
                {
                    DeleteTourButtonClicked?.Invoke(this, SelectedTour);
                }, (_) => SelectedTour != null);
            }
        }
    }

