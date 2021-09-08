using System;
using System.Diagnostics;

namespace FootballApp.Infrastructure.Command.ErrorHandler
{
    class DebugErrorHandler : IErrorHandler
    {
        public void OnException(Exception exception)
        {
            Debug.WriteLine(exception);
        }
    }
}
