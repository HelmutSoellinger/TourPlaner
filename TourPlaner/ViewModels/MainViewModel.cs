﻿using Microsoft.Web.WebView2.Wpf;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text.Json;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using TourPlaner.BL;
using TourPlaner.Models;
using TourPlaner.Views;
using System.Windows.Forms;
using Org.BouncyCastle.Utilities;
using TourPlaner.logging;


namespace TourPlaner.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private static readonly ILoggerWrapper logger = LoggerFactory.GetLogger(); //Declaring Logger
        private readonly EditButtonViewModel logButtonsViewModel;
        private readonly EditButtonViewModel tourButtonsViewModel;

        public EditButtonViewModel LogButtonsViewModel => logButtonsViewModel;
        public EditButtonViewModel TourButtonsViewModel => tourButtonsViewModel;
        public readonly ITourManager tourManager;

        public WebView2 webView;

        private LogModel? selectedLog;
        private TourModel? selectedTour;

        public bool IsTourSelected => SelectedTour != null; // checking if tour is selected

        public ICommand PdfGenerierenCommand { get; }
        public ICommand TourImport { get; }
        public RelayCommand TourExport { get; }

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
                UpdateLeaflet();
                OnPropertyChanged(nameof(SelectedTour));
                OnPropertyChanged(nameof(IsTourSelected));
                TourExport.RaiseCanExecuteChanged();
            }
        }
        
        public async void UpdateLeaflet()
        {
            if (webView == null) //should never be the case but unit tests trigger this...
                return;
            if (webView.CoreWebView2 == null)
                return;
            if (SelectedTour == null)
                return;
            string appDir = AppDomain.CurrentDomain.BaseDirectory;
            string fullPath = System.IO.Path.Combine(appDir, "Resources", SelectedTour.FileName);
            if (!File.Exists(fullPath))
            {

                logger.Debug("File Not Found!");

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
                    new LogModel() { Date = DateTime.Today.ToString(), TotalTime = "4:23", TotalDistance = "8,5 km" },
                    new LogModel() { Date = DateTime.Now.ToString(), TotalTime = "4:12", TotalDistance = "8,1 km" },
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
                    new LogModel() { Date = DateTime.Today.ToString(), TotalTime = "8:23", TotalDistance = "35,5 km" },
                    new LogModel() { Date = DateTime.Now.ToString(), TotalTime = "8:12", TotalDistance = "36,1 km" },
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
                    new LogModel() { Date = DateTime.Today.ToString(), TotalTime = "12:23", TotalDistance = "55,5 km" },
                    new LogModel() { Date = DateTime.Now.ToString(), TotalTime = "12:12", TotalDistance = "57,1 km" },
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
                    new LogModel() { Date = DateTime.Today.ToString(), TotalTime = "3:23", TotalDistance = "7,5 km" },
                    new LogModel() { Date = DateTime.Now.ToString(), TotalTime = "3:12", TotalDistance = "7,1 km" },
                }
            },
        };

        public MainViewModel(ITourManager _tourManager, EditButtonViewModel logButtonsViewModel, EditButtonViewModel tourButtonsViewModel)
        {
            tourManager = _tourManager;
            this.logButtonsViewModel = logButtonsViewModel;
            this.tourButtonsViewModel = tourButtonsViewModel;
            tourManager.Tours = Tours; //pass Tours Collection zum tour manager damit man hier nicht 2 mal die tours adden muss
            PdfGenerierenCommand = new RelayCommand(PdfGenerieren, (_) => SelectedTour != null);
            TourImport = new RelayCommand(ImportTour);
            TourExport = new RelayCommand(ExportTour, (_) => SelectedTour != null);
            var logger = LoggerFactory.GetLogger();


            var dbTours = tourManager.RetrieveTours();
            foreach (var tour in dbTours)
            {
                if (!Tours.Contains(tour)) // Check to avoid duplicates
                    Tours.Add(tour);
            }

            logButtonsViewModel.AddLogButtonClicked += (sender, log) =>
            {
                logger.Debug("Adding new log: " + log.Date);
                if (SelectedTour != null)
                {
                    tourManager.AddLog(SelectedTour.Id, log);
                    OnPropertyChanged(nameof(SelectedTour)); // Notify the UI of the change
                }
                logButtonsViewModel.NewLogName = "";
            };
            logButtonsViewModel.DeleteLogButtonClicked += (sender, log) =>
            {
                logger.Debug($"Deleting log: {log?.Date}");
                if (SelectedTour != null && log != null)
                {
                    SelectedTour.Logs.Remove(log);
                    tourManager.RemoveLog(log);
                    OnPropertyChanged(nameof(SelectedTour)); // Notify the UI of the change
                }
            };
            logButtonsViewModel.ModifyLogButtonClicked += (sender, log) =>
            {
                logger.Debug($"Modify log: {log?.Date}");
                if (SelectedTour != null && log != null)
                {
                    tourManager.Update();
                    OnPropertyChanged(nameof(SelectedTour)); // Notify the UI of the change
                }
            };
            tourButtonsViewModel.AddTourButtonClicked += async (sender, tour) =>
            {
                if (tour.FileName == null)
                {
                    string filename = await APICall.Call(tour.StartLocation, tour.EndLocation);
                    tour.FileName = filename;
                    string appDir = AppDomain.CurrentDomain.BaseDirectory;
                    string fullPath = System.IO.Path.Combine(appDir, "Resources", filename);
                    if (!File.Exists(fullPath))
                    {
                        logger.Debug("File Not Found!");
                    }
                    else
                    {
                        var json = File.ReadAllText(fullPath);
                        var data = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(json);
                        try
                        {
                            float a = data.features[0].properties.segments[0].distance;
                            tour.Distance = $"{a}m";
                        }
                        catch {
                            logger.Debug("Couldnt Parse Json!");
                        }
                    }
                }
                tourManager.AddTour(tour);
                logger.Debug("Adding new tour: " + tour.Name);
                OnPropertyChanged(nameof(Tours));
                tourButtonsViewModel.NewTourName = "";
            };
            tourButtonsViewModel.DeleteTourButtonClicked += (sender, tour) =>
            {
                logger.Debug($"Deleting tour: {tour?.Name}");
                string filename = $"./Resources/{tour.FileName}";
                if (tour != null)
                {
                    if (File.Exists(filename))
                        File.Delete(filename);
                    tourManager.RemoveTour(tour);
                    OnPropertyChanged(nameof(Tours));
                }
            };
            tourButtonsViewModel.ModifyTourButtonClicked += (sender, tour) =>
            {
                logger.Debug($"Modify tour: {tour?.Name}");
                if (tour != null) //kein ahnung warum tour null sein sollte, hab ich den null-check geadded?
                {
                    tourManager.Update();
                    OnPropertyChanged(nameof(Tours));
                }
            };
        }
        public MainViewModel()  // only necessary for the designer
        {
            logButtonsViewModel = new EditButtonViewModel();
            tourButtonsViewModel = new EditButtonViewModel();

        }
        private void PdfGenerieren(object parameter)
        {
            var selectedItem = SelectedTour;
            if (selectedItem != null)
            {
                // Create an instance of PdfGenerator
                var pdfGenerator = new PdfGenerator();

                // Call GeneratePdfForTour on the instance of PdfGenerator
                pdfGenerator.GeneratePdfForTour(selectedItem);
            }
            else
            {
                logger.Debug("Kein TourModell ausgewählt.");
            }
        }

        private void ImportTour(object parameter)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

            // Set filter for file extension and default file extension 
            dlg.DefaultExt = ".tour";
            dlg.Filter = "Tour Files (*.tour)|*.tour";

            // Display OpenFileDialog by calling ShowDialog method 
            Nullable<bool> result = dlg.ShowDialog();

            // Get the selected file name and display in a TextBox 
            if (result == true)
            {
                // Open document 
                string filename = dlg.FileName;
                if (!File.Exists(filename))
                {
                    throw new FileNotFoundException($"The file {filename} does not exist.");
                }

                string jsonString = File.ReadAllText(filename);
                var tour = JsonSerializer.Deserialize<TourModel>(jsonString);
                tour.Id = 0;
                foreach (var log in tour.Logs)
                {
                    log.Id = 0;
                }
                tourManager.AddTour(tour);
                logger.Debug("Adding new tour: " + tour.Name);
                OnPropertyChanged(nameof(Tours));
                tourButtonsViewModel.NewTourName = "";
            }
        }
        private void ExportTour(object parameter)
        {
            using var fbd = new FolderBrowserDialog();
            
            DialogResult result = fbd.ShowDialog();

            if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
            {
                var tourModel = SelectedTour;
                var options = new JsonSerializerOptions
                {
                    WriteIndented = true // Makes the output more readable
                };

                string jsonString = JsonSerializer.Serialize(tourModel, options);
                File.WriteAllText($"{fbd.SelectedPath}/{tourModel.Name}.tour", jsonString);
            }
        }
    }
}
