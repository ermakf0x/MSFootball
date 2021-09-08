using FootballApp.Infrastructure.Command.ErrorHandler;
using System;
using System.Threading.Tasks;

namespace FootballApp.Infrastructure.Command
{
    abstract class AsyncCommandBase : CommandBase
    {
        protected readonly IErrorHandler _errorHandler;

        public bool Executing { get; private set; }

        protected AsyncCommandBase(IErrorHandler errorHandler)
        {
            _errorHandler = errorHandler;
        }

        public override bool CanExecute(object parameter) => !Executing;
        public override async void Execute(object parameter)
        {
            try
            {
                Executing = true;
                await ExecuteAsync(parameter);
            }
            catch (Exception ex)
            {
                _errorHandler?.OnException(ex);
            }
            Executing = false;
            ReisePropertyChanged();
        }

        public abstract Task ExecuteAsync(object parameter);
    }
}
