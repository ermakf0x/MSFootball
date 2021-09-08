using System;

namespace FootballApp.Infrastructure.Command
{
    class DelegeteCommand : CommandBase
    {
        readonly Action _execute;
        readonly Func<bool> _canExecute;

        public DelegeteCommand(Action execute, Func<bool> canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public override bool CanExecute(object parameter) => _canExecute?.Invoke() ?? true;
        public override void Execute(object parameter) => _execute.Invoke();
    }
    class DelegeteCommand<T> : CommandBase
    {
        readonly Action<T> _execute;
        readonly Func<T, bool> _canExecute;

        public DelegeteCommand(Action<T> execute, Func<T, bool> canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public override bool CanExecute(object parameter) => _canExecute?.Invoke((T)parameter) ?? true;
        public override void Execute(object parameter) => _execute.Invoke((T)parameter);
    }
}
