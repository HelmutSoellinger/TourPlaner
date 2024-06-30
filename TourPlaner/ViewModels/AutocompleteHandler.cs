using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourPlaner.BL;
using TourPlaner.Models;

namespace TourPlaner.ViewModels
{
    public class AutocompleteHandler : BaseViewModel
    {
        private string _location;
        public bool DropDown { get; set; } = false;
        public ObservableCollection<string> LocationsComplete { get; set; } = new();
        public string Location
        {
            get { return _location; }
            set
            {
                _location = value;
                DropDown = true;
                OnPropertyChanged(nameof(DropDown));
                AsyncSetterAPICall(value);
            }
        }
        async private void AsyncSetterAPICall(string value)
        {
            var completion = await APICall.Complete(value);
            LocationsComplete = completion.GetObservable();
            OnPropertyChanged(nameof(LocationsComplete));
        }
    }
}
