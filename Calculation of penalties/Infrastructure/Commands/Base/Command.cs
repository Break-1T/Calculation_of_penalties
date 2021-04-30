using System;
using System.Windows.Input;

namespace Calculation_of_penalties.Infrastructure.Commands.Base
{
    internal abstract class Command:ICommand
    {
        //Чи може виконуватися команда
        public abstract bool CanExecute(object parameter);

        //Що виконує команда
        public abstract void Execute(object parameter);

        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }
    }
}
