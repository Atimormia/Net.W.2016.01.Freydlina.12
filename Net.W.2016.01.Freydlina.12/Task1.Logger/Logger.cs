using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task1.Logger
{
    public class Logger: ILogger
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public void Error(string message) => logger.Error(message);

        public void Info(string message) => logger.Info(message);

        public void Warn(string message) => logger.Warn(message);

        public void Debug(string message) => logger.Debug(message);
    }
}
