using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using TourPlaner.Models;

namespace TourPlaner.ViewModels
{
   
        internal class EditButtonViewModel : BaseViewModel
        {
            private string newTourName = "New Tour";

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

            private TourModel? selectedTour;

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


            public event EventHandler<TourModel>? AddButtonClicked;

            public event EventHandler<TourModel?>? DeleteButtonClicked;


            public EditButtonViewModel()
            {
                AddNewTourCommand = new RelayCommand((_) =>
                {
                    AddButtonClicked?.Invoke(this, new TourModel() { Date = DateTime.Now });
                }, (_) => !string.IsNullOrWhiteSpace(NewTourName));

                DeleteTourCommand = new RelayCommand((_) =>
                {
                    DeleteButtonClicked?.Invoke(this, SelectedTour);
                }, (_) => SelectedTour != null);
            }
        }
    }

