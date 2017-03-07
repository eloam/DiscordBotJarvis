using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DiscordBotJarvis.Enums;
namespace DiscordBotJarvis.Core
{

    public class Logger
    {
        // Properties
        public string LogFilePath { get; }

        public LogLevel LogLevelDisplay { get; set; }

        public LogLevel LogLevelDisplayConsole { get; set; }

        // Constructor
        public Logger(string logFileName, LogLevel logLevelDisplay = LogLevel.Verbose, LogLevel logLevelDisplayConsole = LogLevel.Error)
        {
            LogFilePath = logFileName;
            LogLevelDisplay = logLevelDisplay;
            LogLevelDisplayConsole = logLevelDisplayConsole;
        }

        // Methods
        public void Log(LogLevel logLevel, string message, bool showErrorConsole = true, bool showDateTime = true)
        {
            if (logLevel >= LogLevelDisplay)
            {
                StringBuilder contents = new StringBuilder();

                if (showDateTime)
                    contents.Append($"[{DateTime.Now}]").Append(" ");

                contents.Append($"[{logLevel}] {message}");

                if (showErrorConsole && logLevel >= LogLevelDisplayConsole)
                        Console.WriteLine(contents.ToString());

                File.AppendAllText(LogFilePath, contents.AppendLine().ToString(), Encoding.UTF8);
            }
        }

        public void Verbose(string message, bool showErrorConsole = true, bool showDateTime = true)
        {
            Log(LogLevel.Verbose, message, showErrorConsole, showDateTime);
        }

        public void Info(string message, bool showErrorConsole = true, bool showDateTime = true)
        {
            Log(LogLevel.Info, message, showErrorConsole, showDateTime);
        }

        public void Warning(string message, bool showErrorConsole = true, bool showDateTime = true)
        {
            Log(LogLevel.Warning, message, showErrorConsole, showDateTime);
        }

        public void Error(string message, bool showErrorConsole = true, bool showDateTime = true)
        {
            Log(LogLevel.Error, message, showErrorConsole, showDateTime);
        }

        public void Fatal(string message, bool showErrorConsole = true, bool showDateTime = true)
        {
            Log(LogLevel.Fatal, message, showErrorConsole, showDateTime);
        }
    }
}
