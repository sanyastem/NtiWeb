using System;
using log4net;

namespace ASUVP.Core.Logging
{
    public class EventLogger : IEventLogger
    {
        private readonly ILog _logger;

        public EventLogger(string name)
        {
            _logger = LogManager.GetLogger(name);
        }

        public void Debug(string message)
        {
            _logger.Debug(message);
        }

        public void Info(string message)
        {
            _logger.Info(message);
        }

        public void Warn(string message)
        {
            _logger.Warn(message);
        }

        public void Error(string message)
        {
            _logger.Error(message);
        }

        public void Error(string message, Exception e)
        {
            _logger.Error(message, e);
        }

        public void Fatal(string message)
        {
            _logger.Fatal(message);
        }
    }
}