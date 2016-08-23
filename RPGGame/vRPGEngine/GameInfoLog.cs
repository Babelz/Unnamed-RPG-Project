using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vRPGEngine
{
    public enum InfoLogEntryType : int
    {
        Message = 0,
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
        public string           Contents;
        public string           Data;
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
        
        private InfoLogEntry CreateEntry(string contents, InfoLogEntryType type, object data = null)
        {
            InfoLogEntry entry;

            entry.Contents  = contents;
            entry.Type      = type;
            entry.Data      = data?.ToString();

            return entry;
        }
        
        public void LogRaw(string contents, InfoLogEntryType type)
        {
            entries.Add(CreateEntry(contents, type));
        }

        public void LogMessage(string message)
        {
            entries.Add(CreateEntry(message, InfoLogEntryType.Message));
        }
        public void LogWarning(string warning)
        {
            entries.Add(CreateEntry(warning, InfoLogEntryType.Message));
        }
        public void LogTakeDamage(int amount, bool critical, string fromSpell, string fromEntity)
        {
            var message = string.Format("{0} dealt {1} damage using {2}{3}", fromEntity, amount, fromSpell, critical ? " (critical)" : string.Empty);

            entries.Add(CreateEntry(message, InfoLogEntryType.TakeDamage, amount));
        }
        public void LogDealDamage(int amount, bool critical, string fromSpell, string targetEntity)
        {
            var message = string.Format("{0} dealt {1} damage to {2}{3}", fromSpell, amount, targetEntity, critical ? " (critical)" : string.Empty);

            entries.Add(CreateEntry(message, InfoLogEntryType.DealDamage, amount));
        }
        public void LogGainHealth(int amount, string from, string sender)
        {
            var message = string.Format("{0} gained {1} health from {1}", sender, amount, from);

            entries.Add(CreateEntry(message, InfoLogEntryType.GainHealth, amount));
        }
        public void LogGainMana(int amount, string from, string sender)
        {
            var message = string.Format("{0} gained {1} mana from {1}", sender, amount, from);

            entries.Add(CreateEntry(message, InfoLogEntryType.GainMana, amount));
        }
        public void LogGainFocus(int amount, string from, string sender)
        {
            var message = string.Format("{0} gained {1} focus from {1}", sender, amount, from);

            entries.Add(CreateEntry(message, InfoLogEntryType.GainFocus, amount));
        }
        public void LogGainReputation(int amount, string faction)
        {
            var message = string.Empty;

            if (amount < 0) message = string.Format("Lost {0} reputation towards {1}", Math.Abs(amount), faction);
            else            message = string.Format("Gained {0} reputation towards {1}", amount, faction);

            entries.Add(CreateEntry(message, InfoLogEntryType.GainReputation, string.Format("+{0} {1}", amount, faction)));
        }
        public void LogUseSpell(string spell, string sender, string target = "")
        {
            var message = string.Empty;

            if (!string.IsNullOrEmpty(target)) message = string.Format("{0} used spell {1} and is targeting {2}", sender, spell, target);
            else                               message = string.Format("{0} used spell {1}", sender, spell);

            entries.Add(CreateEntry(message, InfoLogEntryType.UseSpell, spell));
        }
        public void GainBuff(string buff, string sender)
        {
            var message = string.Format("{0} gained buff {1}", sender, buff);

            entries.Add(CreateEntry(message, InfoLogEntryType.GainBuff, buff));
        }
        public void LoseBuff(string buff, string sender)
        {
            var message = string.Format("{0} lost buff {1}", sender, buff);

            entries.Add(CreateEntry(message, InfoLogEntryType.LoseBuff, buff));
        }
        public void GainDebuff(string debuff, string sender)
        {
            var message = string.Format("{0} gained debuff {1}", sender, debuff);

            entries.Add(CreateEntry(message, InfoLogEntryType.GainDebuff, debuff));
        }
        public void LoseDebuff(string debuff, string sender)
        {
            var message = string.Format("{0} lost debuff {1}", sender, debuff);

            entries.Add(CreateEntry(message, InfoLogEntryType.LoseDebuff, debuff));
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
