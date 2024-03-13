using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using TourPlaner.Models;

namespace TourPlaner.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private readonly EditButtonViewModel logButtonsViewModel;
        private readonly EditButtonViewModel tourButtonsViewModel;
       
        private LogModel? selectedLog;
        private TourModel? selectedTour;

        public LogModel? SelectedLog
        {
            get => selectedLog;
            set
            {
                selectedLog = value;
                logButtonsViewModel.SelectedLog = value;
            }
        }

        public TourModel? SelectedTour
        {
            get => selectedTour;
            set
            {
                selectedTour = value;
                if (tourButtonsViewModel != null)
                {
                    tourButtonsViewModel.SelectedTour = value;
                }
                else
                {
                    // Handle the case where tourButtonsViewModel is null
                    // For example: throw an exception or log an error
                }
            }
        }


        public ObservableCollection<LogModel> Logs { get; } = new ObservableCollection<LogModel>()
        {
            new LogModel() { Date = DateTime.Today, TotalTime = "4:23", TotalDistance = "8,5 km" },
            new LogModel() { Date = DateTime.Now, TotalTime = "4:12", TotalDistance = "8,1 km" },
        };

        public ObservableCollection<TourModel> Tours { get; } = new ObservableCollection<TourModel>()
        {
            new TourModel() { Name = "Doberkogel", Description = "Description 1", StartLocation = "Start 1", EndLocation = "End 1", RouteInformation = "Route 1", Distance = "5 km", ImagePath = "Images/1.jpg" },
            new TourModel() { Name = "Straight Line through Austria", Description = "Description 2", StartLocation = "Start 2", EndLocation = "End 2", RouteInformation = "Route 2", Distance = "10 km", ImagePath = "Images/2.jpg" },
            new TourModel() { Name = "SUP to Bratislva", Description = "Description 3", StartLocation = "Start 3", EndLocation = "End 3", RouteInformation = "Route 3", Distance = "20 km", ImagePath = "Images/3.jpg" },
            new TourModel() { Name = "ViennaSightseeing", Description = "Description 4", StartLocation = "Start 4", EndLocation = "End 4", RouteInformation = "Route 4", Distance = "20 km", ImagePath = "Images/4.jpg" },
        };


        public MainViewModel(EditButtonViewModel logButtonsViewModel, EditButtonViewModel tourButtonsViewModel)
        {
            this.logButtonsViewModel = logButtonsViewModel;

            logButtonsViewModel.AddButtonClicked += (sender, log) =>
            {
                Debug.Print("Adding new log: " + log.Date);
                Logs.Add(log);
                OnPropertyChanged(nameof(Logs));

                logButtonsViewModel.NewLogName = "";
            
            };
        
         
            logButtonsViewModel.DeleteButtonClicked += (sender, log) =>
            {
                Debug.Print($"Deleting log: {log?.Date}");
                if (log != null)
                    Logs.Remove(log);
                OnPropertyChanged(nameof(Logs));
               
            };
          
        }
        public MainViewModel()  // only necessary for the designer
        {
            logButtonsViewModel = new EditButtonViewModel();
        }
    }
}
