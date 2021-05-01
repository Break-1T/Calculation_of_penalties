using System;
using System.Threading;
using System.Windows;
using Calculation_of_penalties.Infrastructure.Commands;
using Calculation_of_penalties.Infrastructure.Commands.Base;
using Calculation_of_penalties.Models;
using Calculation_of_penalties.Resources;
using Calculation_of_penalties.Services;
using Calculation_of_penalties.View;
using Microsoft.Win32;

namespace Calculation_of_penalties.ViewModel
{
    class StartWindowViewModel:Base.ViewModel
    {
        public StartWindowViewModel()
        {
            IsConstant = "Hidden";
            SAlimentTotal = "0";
            
            AmountIsConstant = new RelayCommand(OnAmountIsConstantAppCommandExecuted, CanAmountIsConstantAppCommandExecute);
            AmountIsNotConstant = new RelayCommand(OnAmountIsNotConstantAppCommandExecuted, CanAmountIsNotConstantAppCommandExecute);
            OpenData = new RelayCommand(OnOpenDataAppCommandExecuted, CanOpenDataAppCommandExecute);
            OpenLoadDialog = new RelayCommand(OnOpenLoadDialogAppCommandExecuted, CanOpenLoadDialogAppCommandExecute);
            Exit = new RelayCommand(OnExitAppCommandExecuted, CanExitAppCommandExecute);
            Help = new RelayCommand(OnhelpAppCommandExecuted, CanhelpAppCommandExecute);
            About = new RelayCommand(OnAboutAppCommandExecuted, CanAboutAppCommandExecute);
            
            _startDate = new Date();
            _endDate = new Date();
            
            ShowIntro();
        }

        private FileIOService fileio;
        
        public DataBaseView DataView { get; set; }
        public OpenFileDialog OpenFile { get; private set; }

        //Сума аліментів, яку потрібно сплатити
        public string SAlimentTotal
        {
            get => _SAlimentTotal;
            set
            {
                _SAlimentTotal = value.Replace('.',',');
                OnPropertyChanged("SAlimentTotal");
            }
        }
        private string _SAlimentTotal;
        
        public double AlimentTotal
        {
            get => Convert.ToDouble(_SAlimentTotal);
            private set { }
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

        #region Введення дат

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

        #region Комманди

        //Команди, що відповідають за відображення поля вводу сумми аліментів
        public Command AmountIsConstant { get; }
        public Command AmountIsNotConstant { get; }
        //Комманда, що відповідає за створення локальної бази даних
        public Command OpenData { get; }
        //Комманда, що відповідає за загрузку готових таблиць у форматі .json
        public Command OpenLoadDialog { get; }
        //Комманда, що відповідає за закриття програми
        public Command Exit { get; }
        //Комманда, що відповідає за відкриття віконця, про допомогу
        public Command Help { get; }
        //Комманда, що відповідає за відкриття вікна з інформацією про розробника
        public Command About { get; }


        //Методи, що відповідають за те, що роблять команди, та чи можуть вони виконуватися
        private void OnOpenDataAppCommandExecuted(object p)
        {
            try
            {
                double result = Convert.ToDouble(AlimentTotal.ToString());
                DataView = new DataBaseView()
                {
                    DataContext = new DataBaseViewModel(this)
                };
                DataView.Show();
                App.Current.MainWindow.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{MyResources.ConvertError}\n{ex.Message}");
                return;
            }

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
            DataView = new DataBaseView()
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
            IsConstant = "Visible";
            SAlimentTotal = "0";
        }
        private bool CanAmountIsConstantAppCommandExecute(object p)
        {
            return true;
        }
        
        private void OnAmountIsNotConstantAppCommandExecuted(object p)
        {
            IsConstant = "Hidden";
        }
        private bool CanAmountIsNotConstantAppCommandExecute(object p)
        {
            return true;
        }

        private void OnhelpAppCommandExecuted(object p)
        {
            MessageBox.Show("1. Для розрахунку пені спочатку введіть дані початку та кінця стягнення аліментів\n"+
                            "2. У випадку, якщо сума про нараховані аліменти стала - натисніть 'Так', та введіть суму\n" +
                            "3. Після введеня даних натисніть кнопку 'Розрахувати'");
        }
        private bool CanhelpAppCommandExecute(object p)
        {
            return true;
        }

        private void OnAboutAppCommandExecuted(object obj)
        {
            MessageBox.Show("Розробник програмного забезпечення: студент КНТЕУ 2-6 ФІТ Крупко Тарас Сергійович\n" +
                            "Науковий керівник: Бебешко Богдан Тарасович");
        }
        private bool CanAboutAppCommandExecute(object arg)
        {
            return true;
        }
        #endregion

        private void ShowIntro()
        {
            IntroView intro = new IntroView();
            intro.Show();
            Thread.Sleep(5000);
            intro.Close();
        }

    }
}
