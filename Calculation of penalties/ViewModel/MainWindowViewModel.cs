using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Calculation_of_penalties.Infrastructure;
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
            IsConstant = "Hidden";
            AlimentTotal = 0;
            AmountIsConstant = new RelayCommand(OnAmountIsConstantAppCommandExecuted, CanAmountIsConstantAppCommandExecute);
            AmountIsNotConstant = new RelayCommand(OnAmountIsNotConstantAppCommandExecuted, CanAmountIsNotConstantAppCommandExecute);
            OpenData = new RelayCommand(OnOpenDataAppCommandExecuted, CanOpenDataAppCommandExecute);
            OpenLoadDialog = new RelayCommand(OnOpenLoadDialogAppCommandExecuted, CanOpenLoadDialogAppCommandExecute);
            Exit = new RelayCommand(OnExitAppCommandExecuted, CanExitAppCommandExecute);
            
            _startDate = new Date();
            _endDate = new Date();
        }
        private FileIOService fileio;
        
        public DataBase DataView { get; set; }
        public OpenFileDialog OpenFile { get; private set; }

        //Сума аліментів, яку потрібно сплатити
        private double _AlimentTotal;
        public double AlimentTotal
        {
            get => _AlimentTotal;
            set
            {
                _AlimentTotal = value;
                OnPropertyChanged("AlimentTotal");
            }
        }

        //Видимість поля для введення нарахованої суми аліментів
        private string _IsConstant;
        public string IsConstant
        {
            get => _IsConstant;
            set
            {
                _IsConstant = value;
                OnPropertyChanged("IsConstant");
            }
        }

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

        //Команди, що відповідають за відображення поля вводу сумми аліментів
        public ICommand AmountIsConstant { get; }
        public ICommand AmountIsNotConstant { get; }
        
        //Комманда, що відповідає за створення локальної бази даних
        public ICommand OpenData { get; }

        //Комманда, що відповідає за загрузку готових таблиць у форматі .json
        public ICommand OpenLoadDialog { get; }

        //Комманда, що відповідає за закриття програми
        public ICommand Exit { get; }

        //Методи, що відповідають за те, що роблять команди, та чи можуть вони виконуватися
        private void OnOpenDataAppCommandExecuted(object p)
        {
            DataView = new DataBase()
            {
                DataContext = new DataBaseViewModel(this)
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
                DataContext = new DataBaseViewModel(this)
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

        private void OnAmountIsConstantAppCommandExecuted(object p)
        {
            IsConstant = "Hidden";
            AlimentTotal = 0;
        }
        private bool CanAmountIsConstantAppCommandExecute(object p)
        {
            return true;
        }
        
        private void OnAmountIsNotConstantAppCommandExecuted(object p)
        {
            IsConstant = "Visible";
        }
        private bool CanAmountIsNotConstantAppCommandExecute(object p)
        {
            return true;
        }
        #endregion

    }
}
