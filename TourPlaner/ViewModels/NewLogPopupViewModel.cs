using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using TourPlaner.Models;
using System.Security.Cryptography;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace TourPlaner.ViewModels
{
    public class NewLogPopupViewModel : BaseViewModel
    {
        private LogModel _logModel;
        private bool _result;
        public RelayCommand OkCommand { get; }
        public RelayCommand CancelCommand { get; }
        public NewLogPopupViewModel(LogModel logModel)
        {
            _logModel = logModel;
            _result = false;

            OkCommand = new RelayCommand((parameter) => {
                if (parameter is Window window)
                {
                foreach (string field in new List<string>{ LogModel.Date, LogModel.TotalTime, LogModel.TotalDistance , LogModel.Comment, LogModel.Difficulty, LogModel.Rating})
                {
                    if(String.IsNullOrEmpty(field)) {
                        Result = false;
                        window.Close();
                        return;
                    }
                }
                Result = true;
                LogModel.Difficulty=Constricter(window, LogModel.Difficulty);
                LogModel.Rating=Constricter(window, LogModel.Rating);
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
        private string Constricter(Window window, string field)
        {
            if (float.TryParse(field.Replace(".",","), out float i))
            {
                float a = i > 10 ? 10 : i;
                a = a < 0 ? 0 : a;
                return a.ToString();
            }
            else
            {
                Result = false;
                window.Close();
                return "";
            }
        } 
        public NewLogPopupViewModel() : this(new LogModel())
        { }
        public LogModel LogModel
        {
            get { return _logModel; }
            set
            {
                _logModel = value;
                OnPropertyChanged(nameof(LogModel));
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
    }
}
