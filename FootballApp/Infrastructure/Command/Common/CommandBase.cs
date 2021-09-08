using System;
using System.Windows.Input;

namespace FootballApp.Infrastructure.Command
{
    abstract class CommandBase : ICommand
    {
        public virtual event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }
        public virtual bool CanExecute(object parameter) => true;
        public abstract void Execute(object parameter);

        protected virtual void ReisePropertyChanged()
        {
            CommandManager.InvalidateRequerySuggested();
        }
    }
}
