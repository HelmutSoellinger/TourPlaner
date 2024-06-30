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
        public NewLogPopupViewModel(LogModel logModel)
        {
            _logModel = logModel;
            _result = false;

            OkCommand = new RelayCommand((parameter) => {
                Result = true;
                LogModel.Difficulty = Constricter(LogModel.Difficulty);
                LogModel.Rating = Constricter(LogModel.Rating);
                if (!DateTime.TryParse(LogModel.Date, out DateTime _))
                    Result = false;
                foreach (string field in new List<string> { LogModel.Date, LogModel.TotalTime, LogModel.TotalDistance, LogModel.Comment, LogModel.Difficulty, LogModel.Rating })
                {
                    if (String.IsNullOrEmpty(field))
                    {
                        Result = false;
                    }
                }
                if (parameter is Window window)
                    window.Close();
            });
            CancelCommand = new RelayCommand(
                (parameter) =>
                {
                    Result = false;
                    if (parameter is Window window)
                    {
                        window.Close();
                    }
                });
        }
        private string Constricter(string field)
        {
            if (float.TryParse(field.Replace(".",","), out float i))
            {
                float a = i > 10 ? 10 : i;
                a = a < 0 ? 0 : a;
                return a.ToString();
            }
            else
            {
                return "";
            }
        } 
        public NewLogPopupViewModel() : this(new LogModel())
        { }
    }
}
