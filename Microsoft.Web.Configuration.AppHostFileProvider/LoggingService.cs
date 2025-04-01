using System;
using Microsoft.Extensions.Logging;

namespace JexusManager
{
    public static class LogHelper
    {
        private static ILoggerFactory _loggerFactory;

        public static void Initialize(ILoggerFactory loggerFactory)
        {
            _loggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
        }

        public static ILogger<T> GetLogger<T>()
        {
            if (_loggerFactory == null)
            {
                throw new InvalidOperationException("LoggerFactory has not been initialized.");
            }

            return _loggerFactory.CreateLogger<T>();
        }

        public static ILogger GetLogger(string categoryName)
        {
            if (_loggerFactory == null)
            {
                throw new InvalidOperationException("LoggerFactory has not been initialized.");
            }

            return _loggerFactory.CreateLogger(categoryName);
        }
    }
}
