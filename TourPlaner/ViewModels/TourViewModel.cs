using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using TourPlaner.Models;

namespace TourPlaner.ViewModels
{
    public class TourViewModel : BaseViewModel
    {
        private readonly EditButtonViewModel logButtonsViewModel;
        private readonly EditButtonViewModel tourButtonsViewModel;
        /* private DateTime newTourDate = DateTime.Now;

         public DateTime NewTourDate
         {
             get => newTourDate;
             set
             {
                 newTourDate = value;
                 AddNewTourCommand.RaiseCanExecuteChanged(); // inform the [Add] Button, that the enabled/disabled state should update
             }
         }
        */
        private TourModel? selectedTour;

        public TourModel? SelectedTour
        {
            get => selectedTour;
            set
            {
                selectedTour = value;
                logButtonsViewModel.SelectedTour = value;
                //DeleteTourCommand.RaiseCanExecuteChanged(); // inform the [Delete] Button, that the enabled/disabled state should update
            }
        }

        public ObservableCollection<TourModel> Tours { get; } = new ObservableCollection<TourModel>()
        {
            new TourModel() { Date = DateTime.Today, TotalTime = "4:23", TotalDistance = "8,5 km" },
            new TourModel() { Date = DateTime.Now, TotalTime = "4:12", TotalDistance = "8,1 km" },
        };

        //public event PropertyChangedEventHandler? PropertyChanged;

       /// <summary>
       //public RelayCommand DeleteTourCommand { get; }
       /// </summary>

        public TourViewModel(EditButtonViewModel logButtonsViewModel, EditButtonViewModel tourButtonsViewModel)
        {
            this.logButtonsViewModel = logButtonsViewModel;

            // AddNewTourCommand = new RelayCommand((_) =>
            logButtonsViewModel.AddButtonClicked += (sender, tour) =>
            {
                Debug.Print("Adding new tour: " + tour.Date);
                Tours.Add(tour);
                OnPropertyChanged(nameof(Tours));

                logButtonsViewModel.NewTourName = "";
                /*
                Debug.Print("Adding new Tour: " + NewTourDate);
                Tours.Add(new TourModel() { Date = NewTourDate, TotalDistance = "", TotalTime = "" });
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Tours)));

                NewTourDate = DateTime.Now;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(newTourDate)));
                */
            };
        
          //  );

            // DeleteTourCommand = new RelayCommand((_) =>
            logButtonsViewModel.DeleteButtonClicked += (sender, tour) =>
            {
                Debug.Print($"Deleting tour: {tour?.Date}");
                if (tour != null)
                    Tours.Remove(tour);
                OnPropertyChanged(nameof(Tours));
                /*
                Debug.Print($"Deleting Tour: {SelectedTour}");
                if (SelectedTour != null)
                    Tours.Remove(SelectedTour);
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Tours)));
                */
            };
            //(_) => SelectedTour != null
           // );
        }
        public TourViewModel()  // only necessary for the designer
        {
            logButtonsViewModel = new EditButtonViewModel();
        }
    }
}
