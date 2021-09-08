using System;

namespace FootballApp.Infrastructure.Command.ErrorHandler
{
    interface IErrorHandler
    {
        void OnException(Exception exception);
    }
}
