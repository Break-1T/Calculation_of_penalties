using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
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
            get
            {
                try
                {
                    return new DateTime(Convert.ToInt32(Year), Convert.ToInt32(Month), Convert.ToInt32(Day));

                }
                catch(Exception ex)
                {
                    MessageBox.Show($"Проверьте правильность введённых данных\n{ex.Message}");
                    return new DateTime();
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
