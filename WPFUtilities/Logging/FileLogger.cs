using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFUtilities.Logging
{
    public class FileLogger : ILogger
    {
        private const string DEFAULT_FILE_FORMAT = "log_{0}.txt";
        private const string DATETIME_FORMAT = "yyyy_MM_dd-HH_mm";
        private string filename;

        public FileLogger()
        {
            createFileLogger();
        }

        private void createFileLogger()
        {
            filename = String.Format(DEFAULT_FILE_FORMAT, DateTime.Now.ToString(DATETIME_FORMAT));
        }

        public void Log(string message)
        {
            File.AppendAllText(filename, generateLogMessage(message));
        }

        private string generateLogMessage(string message)
        {
            return String.Format("{0} - {1}\n", DateTime.Now, message);
        }
    }
}
