using System;

namespace ASUVP.Core.Logging
{
    public interface IEventLogger
    {
        void Debug(string message);
        void Info(string message);
        void Warn(string message);
        void Error(string message);
        void Error(string message, Exception e);
        void Fatal(string message);
    }
}