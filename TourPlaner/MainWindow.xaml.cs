using System.Collections.ObjectModel;
using System;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.ComponentModel;

namespace TourPlaner
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ObservableCollection<TourLog> Log { get; set; }
        public MainWindow(DataGrid dataGrid)
        {
            

            // Beispiel-Daten hinzufügen
            Log = new ObservableCollection<TourLog>
            {
            new TourLog { Date = "3.1.24", TotalDistance = "30 km", TotalTime = "5 hours"},
            new TourLog { Date = "2.3.24", TotalDistance = "25 km", TotalTime = "4 hours"},
        };

            DataContext = this;
            dataGrid.Items.Refresh();
        }
    }
    


        public class TourLog : INotifyPropertyChanged
        {
            private string _date;
            private string _totalTime;
            private string _totalDistance;

            public string Date
            {
                get { return _date; }
                set
                {
                    if (_date != value)
                    {
                        _date = value;
                        OnPropertyChanged(nameof(Date));
                    }
                }
            }

            public string TotalTime
            {
                get { return _totalTime; }
                set
                {
                    if (_totalTime != value)
                    {
                        _totalTime = value;
                        OnPropertyChanged(nameof(TotalTime));
                    }
                }
            }

            public string TotalDistance
            {
                get { return _totalDistance; }
                set
                {
                    if (_totalDistance != value)
                    {
                        _totalDistance = value;
                        OnPropertyChanged(nameof(TotalDistance));
                    }
                }
            }

            public event PropertyChangedEventHandler PropertyChanged;

            protected virtual void OnPropertyChanged(string propertyName)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }


