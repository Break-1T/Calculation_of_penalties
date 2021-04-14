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
            OpenSaveDialog = new RelayCommand(OnOpenSaveDialogApplicationCommandExecuted,
                CanOpenSaveDialogApplicationCommandExecute);
            OpenLoadDialog = new RelayCommand(OnOpenLoadDialogApplicationCommandExecuted,
                CanOpenLoadDialogApplicationCommandExecute);
        }

        public CreateDataBase Data { get; set; }
        private FileIOService fileio;
        
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

        public ICommand OpenSaveDialog { get; set; }
        public ICommand OpenLoadDialog { get; set; }
        public ICommand OpenExportDialog { get; set; }


        private void OnOpenSaveDialogApplicationCommandExecuted(object p)
        {
            SaveFile = new SaveFileDialog();
            SaveFile.ShowDialog();
            fileio = new FileIOService(SaveFile.FileName + ".json");
            fileio.SaveData(Data.GetDataCopy());
        }
        private bool CanOpenSaveDialogApplicationCommandExecute(object p)
        {
            return true;
        }

        private void OnOpenLoadDialogApplicationCommandExecuted(object p)
        {
            OpenFile = new OpenFileDialog();
            OpenFile.ShowDialog();
            fileio = new FileIOService(OpenFile.FileName);
            Data.SetDataCopy(fileio.LoadData());
        }
        private bool CanOpenLoadDialogApplicationCommandExecute(object p)
        {
            return true;
        }


        #endregion
    }
}
