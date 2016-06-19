﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFUtilities.Logging
{
    public class ConsoleLogger : ILogger
    {
        public void Log(string message)
        {
            Console.Out.WriteLineAsync(message);
        }
    }
}
