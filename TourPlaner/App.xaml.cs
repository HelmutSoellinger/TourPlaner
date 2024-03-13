using System.Configuration;
using System.Data;
using System.Windows;
using TourPlaner.ViewModels;
using TourPlaner.Views;

namespace TourPlaner
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            var logButtonsViewModel = new EditButtonViewModel();
            var tourButtonsViewModel = new EditButtonViewModel();
            var mainViewModel = new MainViewModel(logButtonsViewModel,tourButtonsViewModel);
            var mainWindow = new MainWindow 
            {
                DataContext = mainViewModel, 
                LogButtons = { 
                    DataContext = logButtonsViewModel 
                },
                TourButtons =
                {
                    DataContext = tourButtonsViewModel 
                }
            };
            mainWindow.Show();
        }
    }

}
