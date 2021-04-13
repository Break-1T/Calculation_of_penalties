using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows;
using System.Windows.Input;
using Calculation_of_penalties.Infrastructure.Commands;
using Calculation_of_penalties.Models;
using Calculation_of_penalties.View;

namespace Calculation_of_penalties.ViewModel
{
    class MainWindowViewModel:Base.ViewModel
    {
        public MainWindowViewModel()
        {
            OpenCalendar = new RelayCommand(OnOpenCalendarApplicationCommandExecuted, CanOpenCalendarApplicationCommandExecute);
            OpenData = new RelayCommand(OnOpenDataApplicationCommandExecuted, CanOpenDataApplicationCommandExecute);
            
            _startDate = new Date();
            _endDate = new Date();
        }

        public DataBase DataView { get; set; }


        #region Ввод дат

        private Date _startDate;
        private Date _endDate;


        public Date StartDate
        {
            get => _startDate;
            set
            {
                _startDate = value;
                OnPropertyChanged("StartDate");
            }
        }
        public Date EndDate
        {
            get => _endDate;
            set
            {
                _endDate = value;
                OnPropertyChanged("EndDate");
            }
        }

        #endregion

        #region Комманды

        public ICommand OpenData { get; }
        public ICommand OpenCalendar { get; }

        private void OnOpenCalendarApplicationCommandExecuted(object p)
        {

        }
        private bool CanOpenCalendarApplicationCommandExecute(object p)
        {
            return true;
        }
        
        private void OnOpenDataApplicationCommandExecuted(object p)
        {
            DataView = new DataBase()
            {
                DataContext = new DataBaseViewModel(_startDate.GetDateTime, _endDate.GetDateTime)
            };
            DataView.Show();
        }
        private bool CanOpenDataApplicationCommandExecute(object p)
        {
            return true;
        }

        #endregion
    }
}
