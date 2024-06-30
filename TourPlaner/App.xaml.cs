using System.Windows;
using TourPlaner.DAL;
using TourPlaner.ViewModels;
using TourPlaner.Views;
using TourPlaner.logging;
using System.IO; 

namespace TourPlaner
{
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {


            // Holen Sie sich den Logger
            var logger = LoggerFactory.GetLogger();

            try
            {
                var ioCConfig = (IoCContainerConfig)Application.Current.Resources["IoCConfig"];
                var mainViewModel = ioCConfig.MainViewModel;
                var mainWindow = new MainWindow
                {
                    DataContext = mainViewModel,
                    LogButtons =
                    {
                        DataContext = mainViewModel.LogButtonsViewModel
                    },
                    TourButtons =
                    {
                        DataContext = mainViewModel.TourButtonsViewModel
                    }
                };
                mainViewModel.webView = mainWindow.webView;
                mainWindow.Show();

                // Log-Nachrichten schreiben
                logger.Debug("Anwendung gestartet.");
            }
            catch (Exception ex)
            {
                // Fehlerbehandlung und Logging
                logger.Error($"Ein Fehler ist aufgetreten: {ex.Message}");
            }
        }
    }
}
