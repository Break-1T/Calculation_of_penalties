using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Input;
using Calculation_of_penalties.Infrastructure.Commands;

namespace Calculation_of_penalties.ViewModel
{
    class MainWindowViewModel
    {
        public MainWindowViewModel()
        {
            Exit = new RelayCommand(OnExitApplicationCommandExecuted, CanExitApplicationCommandExecute);
        }
        public ICommand Exit { get; }

        private void OnExitApplicationCommandExecuted(object p)
        {
            MessageBox.Show("Hello");
        }
        private bool CanExitApplicationCommandExecute(object p)
        {
            return true;
        }
    }
}
