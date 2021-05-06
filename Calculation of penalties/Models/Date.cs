using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Calculation_of_penalties.Annotations;

namespace Calculation_of_penalties.Models
{
    class Date : INotifyPropertyChanged
    {
        private string _day;
        private string _year;
        private string _month;

        public string Day
        {
            get => _day;
            set
            {
                if (value.Length < 3)
                    _day = value;
                OnPropertyChanged("Day");
            }
        }
        public string Month
        {
            get => _month;
            set
            {
                if (value.Length < 3)
                    _month = value;
                OnPropertyChanged("Month");
            }
        }
        public string Year
        {
            get => _year;
            set
            {
                if (value.Length < 5)
                    _year = value;
                OnPropertyChanged("Year");
            }
        }
        public DateTime GetDateTime
        {
            get => new DateTime(Convert.ToInt32(Year), Convert.ToInt32(Month), Convert.ToInt32(Day));
        }
        public event PropertyChangedEventHandler PropertyChanged;
        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
