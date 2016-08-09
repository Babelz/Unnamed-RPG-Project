using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vRPGEngine
{
    public enum InfoLogEntryType : int
    {
        Message,
        Warning
    }

    public struct InfoLogEntry
    {
        public string Contents;
        public InfoLogEntryType Type;
    }

    public sealed class GameInfoLog : Singleton<GameInfoLog>
    {
        #region Fields
        private readonly List<InfoLogEntry> entries;
        #endregion

        public GameInfoLog()
            : base()
        {
            entries = new List<InfoLogEntry>();
        }

        public void Log(string contents, InfoLogEntryType type)
        {
            InfoLogEntry entry;

            entry.Contents  = contents;
            entry.Type      = type;

            entries.Add(entry);
        }

        public IEnumerable<InfoLogEntry> Entries()
        {
            return entries;
        }

        public void Clear()
        {
            entries.Clear();
        }
    }
}
