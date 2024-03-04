using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;

namespace TourPlaner
{
    internal class TourViewModel : INotifyPropertyChanged
    {
        private DateTime newTourDate = DateTime.Now;

        public DateTime NewTourDate
        {
            get => newTourDate;
            set
            {
                newTourDate = value;
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

        public ObservableCollection<TourModel> Tours { get; } = new ObservableCollection<TourModel>()
        {
            new TourModel() { Date = DateTime.Today, TotalTime = "4:23", TotalDistance = "8,5 km" },
            new TourModel() { Date = DateTime.Now, TotalTime = "4:12", TotalDistance = "8,1 km" },
        };

        public event PropertyChangedEventHandler? PropertyChanged;

        public RelayCommand AddNewTourCommand { get; }
        public RelayCommand DeleteTourCommand { get; }

        public TourViewModel()
        {
            AddNewTourCommand = new RelayCommand((_) =>
            {
                Debug.Print("Adding new Tour: " + NewTourDate);
                Tours.Add(new TourModel() { Date = NewTourDate, TotalDistance = "", TotalTime = "" });
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Tours)));

                NewTourDate = DateTime.Now;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(newTourDate)));
            }
        
            );

            DeleteTourCommand = new RelayCommand((_) =>
            {
                Debug.Print($"Deleting Tour: {SelectedTour}");
                if (SelectedTour != null)
                    Tours.Remove(SelectedTour);
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Tours)));
            },
            (_) => SelectedTour != null
            );
        }
    }
}
