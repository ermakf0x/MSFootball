using FootballApp.Infrastructure.Command.ErrorHandler;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FootballApp.Infrastructure.Command
{
    class AsyncDelegeteCommand : AsyncCommandBase
    {
        readonly Func<Task> _execute;
        readonly Func<bool> _canExecute;

        public AsyncDelegeteCommand(Func<Task> execute, Func<bool> canExecute = null, IErrorHandler errorHandler = null)
            : base(errorHandler)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public override Task ExecuteAsync(object parameter)
        {
            return _execute.Invoke();
        }
    }
}
