using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows;
using System.Windows.Input;
using Calculation_of_penalties.Infrastructure.Commands;
using Calculation_of_penalties.Models;
using Calculation_of_penalties.Services;
using Calculation_of_penalties.View;
using Microsoft.Win32;

namespace Calculation_of_penalties.ViewModel
{
    class MainWindowViewModel:Base.ViewModel
    {
        public MainWindowViewModel()
        {
            OpenCalendar = new RelayCommand(OnOpenCalendarAppCommandExecuted, CanOpenCalendarAppCommandExecute);
            OpenData = new RelayCommand(OnOpenDataAppCommandExecuted, CanOpenDataAppCommandExecute);
            OpenLoadDialog = new RelayCommand(OnOpenLoadDialogAppCommandExecuted, CanOpenLoadDialogAppCommandExecute);
            Exit = new RelayCommand(OnExitAppCommandExecuted, CanExitAppCommandExecute);
            
            _startDate = new Date();
            _endDate = new Date();
        }

        public DataBase DataView { get; set; }
        public OpenFileDialog OpenFile { get; private set; }

        #region Ввод дат

        private Date _startDate;
        private Date _endDate;
        private FileIOService fileio;

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
        public ICommand OpenLoadDialog { get; }
        public ICommand Exit { get; }
        
        private void OnOpenCalendarAppCommandExecuted(object p)
        {

        }
        private bool CanOpenCalendarAppCommandExecute(object p)
        {
            return true;
        }
        
        private void OnOpenDataAppCommandExecuted(object p)
        {
            DataView = new DataBase()
            {
                DataContext = new DataBaseViewModel(_startDate.GetDateTime, _endDate.GetDateTime)
            };
            DataView.Show();
            App.Current.MainWindow.Close();
        }
        private bool CanOpenDataAppCommandExecute(object p)
        {
            return true;
        }

        private void OnOpenLoadDialogAppCommandExecuted(object p)
        {
            #region Avoiding exception

            StartDate.Day = "1";
            StartDate.Month = "1";
            StartDate.Year = "1";
            EndDate.Day = "1";
            EndDate.Month = "1";
            EndDate.Year = "1";

            #endregion

            OpenFile = new OpenFileDialog();
            OpenFile.ShowDialog();
            fileio = new FileIOService(OpenFile.FileName);
            DataView = new DataBase()
            {
                DataContext = new DataBaseViewModel(_startDate.GetDateTime, _endDate.GetDateTime)
            };
            ((DataBaseViewModel) DataView.DataContext).Data.SetDataCopy(fileio.LoadData());
            DataView.Show();
            App.Current.MainWindow.Close();
        }
        private bool CanOpenLoadDialogAppCommandExecute(object p)
        {
            return true;
        }
        
        private void OnExitAppCommandExecuted(object p)
        {
            Application.Current.MainWindow.Close();
        }
        private bool CanExitAppCommandExecute(object p)
        {
            return true;
        }
        #endregion

    }
}
