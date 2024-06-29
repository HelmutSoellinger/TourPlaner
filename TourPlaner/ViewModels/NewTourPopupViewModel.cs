using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using TourPlaner.Models;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Controls;
using System.Reflection;
using TourPlaner.BL;
using Newtonsoft.Json.Linq;
using System.Windows.Resources;

namespace TourPlaner.ViewModels
{
    public class NewTourPopupViewModel : BaseViewModel
    {
        private TourModel _tourModel;
        private bool _result;
        public RelayCommand OkCommand { get; }
        public RelayCommand CancelCommand { get; }
        public AutocompleteHandler StartHandler { get; } = new();
        public AutocompleteHandler EndHandler { get; } = new();

        public TourModel TourModel
        {
            get { return _tourModel; }
            set
            {
                _tourModel = value;
                OnPropertyChanged(nameof(TourModel));
            }
        }

        public bool Result
        {
            get { return _result; }
            set
            {
                _result = value;
                OnPropertyChanged(nameof(Result));
            }
        }
        public NewTourPopupViewModel(TourModel tourModel)
        {
            TourModel = tourModel;
            _result = false;

            OkCommand = new RelayCommand((parameter) => {
                if (parameter is Window window)
                {
                foreach (string field in new List<string>{ TourModel.Name, TourModel.Description, TourModel.StartLocation, TourModel.EndLocation })
                    {
                        if(String.IsNullOrEmpty(field)) {
                            Result = false;
                            window.Close();
                            return;
                        }
                    }
                    Result = true;
                    window.Close();
                }
            });
            CancelCommand = new RelayCommand(
                (parameter) =>
                {
                    if (parameter is Window window)
                    {
                        Result = false;
                        window.Close();
                    }
                });
        }

        public NewTourPopupViewModel() : this(new TourModel())
        { }
    }
}
