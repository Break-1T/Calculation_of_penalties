using System.Windows;
using Calculation_of_penalties.Infrastructure;
using Calculation_of_penalties.Infrastructure.Commands;
using Calculation_of_penalties.Infrastructure.Commands.Base;
using Calculation_of_penalties.Resources;
using Calculation_of_penalties.Services;
using Microsoft.Win32;

namespace Calculation_of_penalties.ViewModel
{
    class DataBaseViewModel : Base.ViewModel
    {

        public DataBaseViewModel(StartWindowViewModel MainVM)
        {
            this.MainVM = MainVM;
            Data = new CreateDataBase(MainVM.StartDate.GetDateTime, MainVM.EndDate.GetDateTime,this.MainVM.AlimentTotal);
            
            OpenSaveDialog = new RelayCommand(OnOpenSaveDialogAppCommandExecuted,
                CanOpenSaveDialogAppCommandExecute);
            OpenLoadDialog = new RelayCommand(OnOpenLoadDialogAppCommandExecuted,
                CanOpenLoadDialogAppCommandExecute);
            OpenExportDialog =
                new RelayCommand(OnOpenExportDialogAppCommandExecuted, CanOpenExportDialogAppCommandExecute);
            Exit = new RelayCommand(OnExitAppCommandExecuted,CanExitAppCommandExecute);
            Help = new RelayCommand(OnHelpAppCommandExecuted, CanHelpAppCommandExecute);
        }

        public StartWindowViewModel MainVM { get; set; }
        public CreateDataBase Data { get; set; }
        private FileIOService fileio;
        private ExcelHelper excelHelper;

        public SaveFileDialog SaveFile { get; set; }
        public OpenFileDialog OpenFile { get; set; }

        #region Команди
        
        //Комманда, що відповідає за збереження поточних даних у формат .json
        public Command OpenSaveDialog { get;}
        
        //Комманда, що відповідає за загрузку готових таблиць у форматі .json
        public Command OpenLoadDialog { get; }

        //Комманда, що відповідає за збереження поточних даних у файл Excel
        public Command OpenExportDialog { get; }
        
        //Комманда, що відповідає за закриття вікна
        public Command Exit { get; }

        //Комманда, що відповідає за відкриття віконця, про допомогу
        public Command Help { get; }

        //Методи, що відповідають за те, що роблять команди, та чи можуть вони виконуватися
        private void OnOpenSaveDialogAppCommandExecuted(object p)
        {
            SaveFile = new SaveFileDialog();
            SaveFile.ShowDialog();
            fileio = new FileIOService(SaveFile.FileName + ".json");
            fileio.SaveData(Data.GetDataCopy());
        }
        private bool CanOpenSaveDialogAppCommandExecute(object p)
        {
            return true;
        }

        private void OnOpenLoadDialogAppCommandExecuted(object p)
        {
            OpenFile = new OpenFileDialog();
            OpenFile.ShowDialog();
            fileio = new FileIOService(OpenFile.FileName);
            Data.SetDataCopy(fileio.LoadData());
        }
        private bool CanOpenLoadDialogAppCommandExecute(object p)
        {
            return true;
        }

        private void OnOpenExportDialogAppCommandExecuted(object p)
        {
            SaveFile = new SaveFileDialog();
            SaveFile.ShowDialog();
            excelHelper = new ExcelHelper(SaveFile.FileName+".xlsx");
            excelHelper.SaveData(Data.GetDataCopy());
        }
        private bool CanOpenExportDialogAppCommandExecute(object p)
        {
            return true;
        }

        private void OnExitAppCommandExecuted(object p)
        {
            MainVM.DataView.Close();
            App.Current.MainWindow.Show();
        }
        private bool CanExitAppCommandExecute(object p)
        {
            return true;
        }

        private void OnHelpAppCommandExecuted(object p)
        {
            MessageBox.Show(MyResources.DataBaseWindowHelp);
        }
        private bool CanHelpAppCommandExecute(object p)
        {
            return true;
        }

        #endregion
    }
}
