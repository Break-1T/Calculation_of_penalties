using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Input;
using Calculation_of_penalties.Infrastructure;
using Calculation_of_penalties.Infrastructure.Commands;
using Calculation_of_penalties.Models;
using Calculation_of_penalties.Services;
using Microsoft.Win32;

namespace Calculation_of_penalties.ViewModel
{
    class DataBaseViewModel : Base.ViewModel
    {

        public DataBaseViewModel(DateTime start, DateTime end)
        {
            StartTime = start;
            EndTime = end;
            Data = new CreateDataBase(StartTime, EndTime);
            
            OpenSaveDialog = new RelayCommand(OnOpenSaveDialogAppCommandExecuted,
                CanOpenSaveDialogAppCommandExecute);
            OpenLoadDialog = new RelayCommand(OnOpenLoadDialogAppCommandExecuted,
                CanOpenLoadDialogAppCommandExecute);
            OpenExportDialog =
                new RelayCommand(OnOpenExportDialogAppCommandExecuted, CanOpenExportDialogAppCommandExecute);
            Exit = new RelayCommand(OnExitAppCommandExecuted,CanExitAppCommandExecute);
        }

        public CreateDataBase Data { get; set; }
        private FileIOService fileio;
        private ExcelHelper excelHelper;
        
        private bool _setcopy;
        public bool SetCopy
        {
            get => _setcopy;
            set
            {
                _setcopy = value;
                OnPropertyChanged("SetCopy");
            }
        }

        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        
        public SaveFileDialog SaveFile { get; set; }
        public OpenFileDialog OpenFile { get; set; }

        #region Комманды

        public ICommand OpenSaveDialog { get;}
        public ICommand OpenLoadDialog { get; }
        public ICommand OpenExportDialog { get; }
        public ICommand Exit { get; }


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


        #endregion
    }
}
