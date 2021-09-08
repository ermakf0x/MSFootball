using System;
using System.Text;

namespace FootballApp.Infrastructure.Logging
{
    static class Logger
    {
        public enum MessageType
        {
            INFO,
            TRACE,
            DEBUG,
            WARNING,
            ERROR,
            FATAL
        }

        public static void Send(object obj, MessageType type) => Send(obj.ToString(), type);
        public static void Send(string message, MessageType type)
        {
            var builder = new StringBuilder()
                .Append(Enum.GetName(typeof(MessageType), type))
                .Append(": ")
                .Append(message);
            Console.WriteLine(builder.ToString());
        }
    }
}
