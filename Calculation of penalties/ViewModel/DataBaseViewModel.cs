using System.Windows;
using Calculation_of_penalties.Infrastructure;
using Calculation_of_penalties.Infrastructure.Commands;
using Calculation_of_penalties.Infrastructure.Commands.Base;
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
            Help = new RelayCommand(OnhelpAppCommandExecuted, CanhelpAppCommandExecute);
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
            App.Current.Windows[0].Close();
        }
        private bool CanExitAppCommandExecute(object p)
        {
            return true;
        }

        private void OnhelpAppCommandExecuted(object p)
        {
            MessageBox.Show("1. Для розрахунку пені спочатку введіть суму нарахованих та сплачених аліментів. Після введення натисніть 'Enter'\n" +
                            "2. Щоб зберегти поточний: файл натисніть 'Файл'-'Зберегти' та вкажіть місце і назву файлу\n" +
                            "3. Щоб відкрити файл: натисніть 'Файл'-'Відкрити' та виберіть місце і сам файлу\n" +
                            "4. Щоб зберегти поточні дані в файл Excel: натисніть 'Файл'-'Зберегти в Excel' та та вкажіть місце і назву файлу");
        }
        private bool CanhelpAppCommandExecute(object p)
        {
            return true;
        }

        #endregion
    }
}
