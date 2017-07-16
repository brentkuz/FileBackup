using System;
using System.Configuration;
using System.Diagnostics;
using System.Text;

namespace FileBackup.Utility
{
    public interface ILogger
    {
        void Log(EventLogEntryType type, string message = null, Exception ex = null);
    }
    public class Logger : ILogger
    {
        private string sSource = ConfigurationManager.AppSettings["sSource"];
        private string sLog = ConfigurationManager.AppSettings["sLog"];
        public void Log(EventLogEntryType type, string message, Exception ex = null)
        {
            if (!EventLog.SourceExists(sSource))
                EventLog.CreateEventSource(sSource, sLog);
            StringBuilder sb = new StringBuilder();
            if (message != null)
                sb.AppendLine(message);
            if (ex != null)
            {
                sb.AppendLine("Exception: ");
                sb.AppendLine(ex.Message);
                if (ex.InnerException != null)
                {
                    sb.AppendLine("Inner Exception: ");
                    sb.AppendLine(ex.InnerException.Message);
                }
                sb.AppendLine(Environment.NewLine + ex.StackTrace);
            }
            EventLog.WriteEntry(sSource, sb.ToString(), type);
        }
    }
}
