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
        Warning,
        TakeDamage,
        DealDamage,
        GainHealth,
        GainMana,
        GainFocus,
        GainReputation,
        UseSpell,
        GainBuff,
        LoseBuff,
        GainDebuff,
        LoseDebuff
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

        public void LogRaw(string contents, InfoLogEntryType type)
        {
            InfoLogEntry entry;

            entry.Contents  = contents;
            entry.Type      = type;

            entries.Add(entry);
        }

        public void LogMessage(string message)
        {
        }
        public void LogWarning(string warning)
        {
        }
        public void LogTakeDamage(int amount, bool critical = false, string fromSpell = "", string fromEntity = "")
        {
        }
        public void LogDealDamage(int amount, bool critical = false, string fromSpell = "", string targetEntity = "")
        {
        }
        public void LogGainHealth(int amount, string from, string sender)
        {
        }
        public void LogGainMana(int amount, string from, string sender)
        {
        }
        public void LogGainFocus(int amount, string from, string sender)
        {
        }
        public void LogGainReputation(int amount, string faction)
        {
        }
        public void LogUseSpell(string name, string sender, string target = "")
        {
        }
        public void GainBuff(string name, string sender)
        {
        }
        public void LoseBuff(string name, string sender)
        {
        }
        public void GainDebuff(string name, string sender)
        {
        }
        public void LoseDebuff(string name, string sender)
        {
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
