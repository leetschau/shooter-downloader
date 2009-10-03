using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace ShooterDownloader
{
    class FileLogger : ILogger, IDisposable
    {
        //private string _logFilePath = String.Empty;
        private StreamWriter _logWriter = null;

        public FileLogger()
        {
            string logName = Properties.Settings.Default.LogFileName;
            string date = DateTime.Now.ToString("yyMMdd");
            string logFilePath = String.Format("{0}\\{1}_{2}.log", LogDirectory, logName, date);
            _logWriter = new StreamWriter(logFilePath, true);
        }

        #region IDisposable Members

        public void Dispose()
        {
            Dispose(true);

            GC.SuppressFinalize(this);
        }

        #endregion

        #region ILogger Members

        public void Log(string message)
        {
            WriteMessage(message);
        }

        public void Log(string message, params Object[] args)
        {
            string formatMessage = String.Format(message, args);
            WriteMessage(formatMessage);
        }

        #endregion

        //Dispose implementation
        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                _logWriter.Close();
            }
        }

        public void Close()
        {
            Dispose();
        }

        //Log implementation
        private void WriteMessage(string message)
        {
            //sync log writing
            lock (_logWriter)
            {
                //using (StreamWriter sw = new StreamWriter(_logFilePath, true))
                //{
                //    sw.WriteLine(message);
                //}
                _logWriter.WriteLine(message);
                _logWriter.Flush();
            }
        }

        public static string LogDirectory
        {
            get
            {
                return Application.LocalUserAppDataPath;
            }
        }
    }
}
