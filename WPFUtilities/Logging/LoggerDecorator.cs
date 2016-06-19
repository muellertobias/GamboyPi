using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFUtilities.Logging
{
    public class LoggerDecorator : ILogger
    {
        private static LoggerDecorator instance;
        private static object padLock = new object();
        public static LoggerDecorator Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (padLock)
                    {
                        if (instance == null)
                        {
                            instance = new LoggerDecorator();
                        }
                    }
                }
                return instance;
            }
        }

        private List<ILogger> loggingEngines;

        private LoggerDecorator()
        {
            this.loggingEngines = new List<ILogger>();
        }

        public void activate(Loggers logger)
        {
            switch (logger)
            {
                case Loggers.Console: loggingEngines.Add(new ConsoleLogger());
                    break;
                case Loggers.File: loggingEngines.Add(new FileLogger());
                    break;
                default:
                    throw new NotImplementedException(String.Format("Loggers {0} is not implemented in Method activate()", logger.ToString()));
            }
        }

        public void Log(string message)
        {
            if (loggingEngines.Count == 0)
            {
                throw new InvalidOperationException("No Logging-Engine activated!");
            }
            foreach (ILogger logger in loggingEngines)
            {
                logger.Log(message);
            }
        }
    }

    public enum Loggers
    {
        Console,
        File
    }
}
