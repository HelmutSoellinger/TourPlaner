using Microsoft.Web.WebView2.Wpf;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using TourPlaner.BL;
using TourPlaner.Models;
using TourPlaner.Views;

namespace TourPlaner.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private readonly EditButtonViewModel logButtonsViewModel;
        private readonly EditButtonViewModel tourButtonsViewModel;

        public EditButtonViewModel LogButtonsViewModel => logButtonsViewModel;
        public EditButtonViewModel TourButtonsViewModel => tourButtonsViewModel;
        public readonly ITourManager tourManager;

        public WebView2 webView;

        private LogModel? selectedLog;
        private TourModel? selectedTour;

        public bool IsTourSelected => SelectedTour != null; // checking if tour is selected

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
                    logButtonsViewModel.SelectedTour = value; //muss im logbutton auch aktualisiert werden damit der knopf grau wird
                }
                else
                {
                    // Handle the case where tourButtonsViewModel is null
                    // For example: throw an exception or log an error
                }
                UpdateLeaflet();
                OnPropertyChanged(nameof(SelectedTour));
                OnPropertyChanged(nameof(IsTourSelected));   // Notify the UI of the change
            }
        }

        public async void UpdateLeaflet()
        {
            if (webView.CoreWebView2 == null)
                return;
            if (SelectedTour == null)
                return;
            string appDir = AppDomain.CurrentDomain.BaseDirectory;
            string fullPath = System.IO.Path.Combine(appDir,"Resources", SelectedTour.FileName);
            if (!File.Exists(fullPath))
            {
                Debug.WriteLine("File Not Found!");
                await webView.CoreWebView2.ExecuteScriptAsync($"map.remove();var map = L.map('map');");
                return;
            }
            string json = File.ReadAllText(fullPath).Replace("\n", String.Empty);
            await webView.CoreWebView2.ExecuteScriptAsync($"display('{json}');");
        }
public ObservableCollection<TourModel> Tours { get; } = new ObservableCollection<TourModel>()
        {
            new TourModel() 
            {
                Name = "Doberkogel", 
                Description = "Description 1", 
                StartLocation = "Start 1", 
                EndLocation = "End 1", 
                RouteInformation = "Route 1", 
                Distance = "5 km",
                FileName="directions.js",
                //ImagePath = "pack://application:,,,/TourPlaner;component/Images/1.jpg", // URI format for Image Source pack://application:,,,/YourSolution;component/YourImagePath/Image.jpg 
                Logs = new ObservableCollection<LogModel>()
                {
                    new LogModel() { Date = DateTime.Today, TotalTime = "4:23", TotalDistance = "8,5 km" },
                    new LogModel() { Date = DateTime.Now, TotalTime = "4:12", TotalDistance = "8,1 km" },
                } // Initialize Logs for this tour
            }, 
            new TourModel() 
            {
                Name = "Straight Line through Austria", 
                Description = "Description 2", 
                StartLocation = "Start 2", 
                EndLocation = "End 2", 
                RouteInformation = "Route 2", 
                Distance = "10 km",
                FileName="directions_other.js",
                //ImagePath = "pack://application:,,,/TourPlaner;component/Images/2.jpg",
                Logs = new ObservableCollection<LogModel>()
                {
                    new LogModel() { Date = DateTime.Today, TotalTime = "8:23", TotalDistance = "35,5 km" },
                    new LogModel() { Date = DateTime.Now, TotalTime = "8:12", TotalDistance = "36,1 km" },
                }
            },
            new TourModel() 
            { 
                Name = "SUP to Bratislva", 
                Description = "Description 3", 
                StartLocation = "Start 3", 
                EndLocation = "End 3", 
                RouteInformation = "Route 3", 
                Distance = "20 km", 
                FileName = "pack://application:,,,/TourPlaner;component/Images/3.jpg",
                Logs = new ObservableCollection<LogModel>()
                {
                    new LogModel() { Date = DateTime.Today, TotalTime = "12:23", TotalDistance = "55,5 km" },
                    new LogModel() { Date = DateTime.Now, TotalTime = "12:12", TotalDistance = "57,1 km" },
                }
            },
            new TourModel() 
            { 
                Name = "ViennaSightseeing", 
                Description = "Description 4", 
                StartLocation = "Start 4", 
                EndLocation = "End 4", 
                RouteInformation = "Route 4", 
                Distance = "20 km", 
                FileName = "pack://application:,,,/TourPlaner;component/Images/4.jpg",
                Logs = new ObservableCollection<LogModel>()
                {
                    new LogModel() { Date = DateTime.Today, TotalTime = "3:23", TotalDistance = "7,5 km" },
                    new LogModel() { Date = DateTime.Now, TotalTime = "3:12", TotalDistance = "7,1 km" },
                }
            },
        };


        public MainViewModel(ITourManager tourManager, EditButtonViewModel logButtonsViewModel, EditButtonViewModel tourButtonsViewModel)
        {
            this.tourManager = tourManager;
            this.logButtonsViewModel = logButtonsViewModel;

            var dbTours = this.tourManager.retrieveTours();
            foreach (var tour in dbTours)
            {
                if (!Tours.Contains(tour)) // Check to avoid duplicates
                    Tours.Add(tour);
            }

            logButtonsViewModel.AddLogButtonClicked += (sender, log) =>
            {
                Debug.Print("Adding new log: " + log.Date);
                if (SelectedTour != null)
                {
                    this.tourManager.AddLog(SelectedTour.Id, log);
                    OnPropertyChanged(nameof(SelectedTour)); // Notify the UI of the change
                }
                // Logs.Add(log);
                // OnPropertyChanged(nameof(Logs));

                logButtonsViewModel.NewLogName = "";
            };
        
         
            logButtonsViewModel.DeleteLogButtonClicked += (sender, log) =>
            {
                Debug.Print($"Deleting log: {log?.Date}");
                if (SelectedTour != null && log != null)
                {
                    SelectedTour.Logs.Remove(log);
                    this.tourManager.RemoveLog(log);
                    OnPropertyChanged(nameof(SelectedTour)); // Notify the UI of the change
                }
                //  if (log != null)
                //  Logs.Remove(log);
                //  OnPropertyChanged(nameof(Logs));

            };
            
            this.tourButtonsViewModel = tourButtonsViewModel;
            
            tourButtonsViewModel.AddTourButtonClicked += async (sender, tour) =>
            {
                string filename = await APICall.Call(tour.StartLocation, tour.EndLocation);
                tour.FileName= filename;
                this.tourManager.AddTour(tour);
                Debug.Print("Adding new tour: " + tour.Name);
                Tours.Add(tour);
                OnPropertyChanged(nameof(Tours));
                tourButtonsViewModel.NewTourName = "";
            };

            tourButtonsViewModel.DeleteTourButtonClicked += (sender, tour) =>
            {
                Debug.Print($"Deleting tour: {tour?.Name}");
                if (tour != null)
                {
                    this.tourManager.RemoveTour(tour);
                    Tours.Remove(tour);
                    OnPropertyChanged(nameof(Tours));
                }
            };

            tourButtonsViewModel.ModifyTourButtonClicked += (sender, tour) =>
            {
                Debug.Print($"Modify tour: {tour?.Name}");
                if (tour != null)
                {
                    OnPropertyChanged(nameof(Tours));
                }
            };

        }
        public MainViewModel()  // only necessary for the designer
        {
            logButtonsViewModel = new EditButtonViewModel();
            tourButtonsViewModel = new EditButtonViewModel();

        }
    }
}
