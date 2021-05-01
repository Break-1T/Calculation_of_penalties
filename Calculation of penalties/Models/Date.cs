using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Calculation_of_penalties.Annotations;

namespace Calculation_of_penalties.Models
{
    class Date : INotifyPropertyChanged
    {
        public string Day { get; set; }
        public string Month { get; set; }
        public string Year { get; set; }
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
