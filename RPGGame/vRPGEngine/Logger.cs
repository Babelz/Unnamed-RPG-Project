using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace vRPGEngine
{
    public enum LogLevel : int
    {
        Message = 0,
        Warning,
        Error
    }

    public interface ILogger
    {
        #region Properties
        bool InUse
        {
            get;
            set;
        }
        bool HasMessages
        {
            get;
        }
        #endregion

        void Flush();

        void Log(string message, LogLevel level);
    }

    public struct LogMessage
    {
        public LogLevel level;
        public string contents;
    }

    public class LogFormatter
    {
        #region Fields
        private readonly Func<string, string>[] formatters;
        #endregion

        public LogFormatter()
        {
            formatters = new Func<string, string>[]
            {
                FormatMessage,
                FormatWarning,
                FormatError
            };
        }

        private string FormatMessage(string message)
        {
            return string.Format("[MSG]: {0}", message);
        }
        private string FormatWarning(string message)
        {
            return string.Format("[WRN]: {0}", message);
        }
        private string FormatError(string message)
        {
            return string.Format("[ERR]: {0}", message);
        }

        public string Format(string message, LogLevel level)
        {
            return formatters[(int)level](message);
        }
    }

    public sealed class FileLogger : ILogger
    {
        #region Path
        private static readonly string LogPath = "log.txt";
        #endregion

        #region Fields
        private readonly List<LogMessage> buffer;

        private readonly LogFormatter formatter;
        #endregion

        #region Properties
        public bool InUse
        {
            get;
            set;
        }
        public bool HasMessages
        {
            get
            {
                return buffer.Count != 0;
            }
        }
        #endregion

        public FileLogger()
        {
            buffer = new List<LogMessage>();

            formatter = new LogFormatter();

            File.Create(LogPath).Close();
        }

        public void Flush()
        {
            var sb = new StringBuilder();

            foreach (var message in buffer)
            {
                var formatted = formatter.Format(message.contents, message.level);

                sb.Append(formatted);
                sb.Append("\r\n");
            }

            buffer.Clear();

            var output = sb.ToString();

            File.AppendAllText(LogPath, output);
        }

        public void Log(string message, LogLevel level)
        {
            LogMessage msg;
            msg.contents    = message;
            msg.level       = level;

            buffer.Add(msg);
        }
    }

    public sealed class ConsoleLogger : ILogger
    {
        #region Console colors
        private static readonly ConsoleColor[] ConsoleColors = new ConsoleColor[]
        {
            ConsoleColor.Green,
            ConsoleColor.Yellow,
            ConsoleColor.Red
        };
        #endregion

        #region Fields
        private readonly List<LogMessage> buffer;

        private readonly LogFormatter formatter;
        #endregion

        #region Properties
        public bool InUse
        {
            get;
            set;
        }
        public bool HasMessages
        {
            get
            {
                return buffer.Count != 0;
            }
        }
        #endregion

        public ConsoleLogger()
        {
            buffer = new List<LogMessage>();

            formatter = new LogFormatter();
        }

        public void Flush()
        {
            foreach (var message in buffer)
            {
                var formatted   = formatter.Format(message.contents, message.level);
                var color       = ConsoleColors[(int)message.level];

                Console.ForegroundColor = color;

                Console.WriteLine(formatted);

                Console.ResetColor();
            }
            
            buffer.Clear();
        }

        public void Log(string message, LogLevel level)
        {
            LogMessage msg;
            msg.contents    = message;
            msg.level       = level;

            buffer.Add(msg);
        }
    }

    public sealed class Logger : SystemManager<Logger>
    {
        #region Fields
        private readonly ILogger[] loggers;
        #endregion

        private Logger()
            : base()
        {
            loggers = new ILogger[]
            {
                new FileLogger()    { InUse = true },
                new ConsoleLogger() { InUse = true }
            };
        }

        protected override void OnUpdate(GameTime gameTime)
        {
            foreach (var logger in loggers.Where(l => l.InUse && l.HasMessages)) logger.Flush();
        }

        public void LogFunctionMessage(string messge, [CallerLineNumber] int line = 0, [CallerMemberName] string fnc = null)
        {
            foreach (var logger in loggers.Where(l => l.InUse)) logger.Log(string.Format("{0}@{1} - {2}", fnc, line, messge), LogLevel.Message);
        }
        public void LogFunctionWarning(string messge, [CallerLineNumber] int line = 0, [CallerMemberName] string fnc = null)
        {
            foreach (var logger in loggers.Where(l => l.InUse)) logger.Log(string.Format("{0}@{1} - {2}", fnc, line, messge), LogLevel.Warning);
        }
        public void LogFunctionError(string messge, [CallerLineNumber] int line = 0, [CallerMemberName] string fnc = null)
        {
            foreach (var logger in loggers.Where(l => l.InUse)) logger.Log(string.Format("{0}@{1} - {2}", fnc, line, messge), LogLevel.Error);
        }

        public void LogMessage(string message)
        {
            foreach (var logger in loggers.Where(l => l.InUse)) logger.Log(message, LogLevel.Message);
        }
        public void LogWarning(string message)
        {
            foreach (var logger in loggers.Where(l => l.InUse)) logger.Log(message, LogLevel.Warning);
        }
        public void LogError(string message)
        {
            foreach (var logger in loggers.Where(l => l.InUse)) logger.Log(message, LogLevel.Error);
        }
    }
}