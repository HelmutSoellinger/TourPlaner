using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using TourPlaner.DAL;
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
            var ioCConfig = (IoCContainerConfig)Application.Current.Resources["IoCConfig"];
            var mainViewModel = ioCConfig.MainViewModel;
            var mainWindow = new MainWindow 
            {
                DataContext = mainViewModel, 
                LogButtons = { 
                    DataContext = mainViewModel.LogButtonsViewModel
                },
                TourButtons =
                {
                    DataContext = mainViewModel.TourButtonsViewModel
                }
            };
            mainViewModel.webView = mainWindow.webView;
            mainWindow.Show();
        }
    }

}
