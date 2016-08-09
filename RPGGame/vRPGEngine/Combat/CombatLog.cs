using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vRPGEngine.Combat
{
    [Flags()]
    public enum CombatLogEntryType : int
    {
        None        = 0,
        Critical    = 1,
        DealDamage  = 2,
        TakeDamage  = 4,
        Heal        = 8,
        GainBuff    = 16,
        LoseBuff    = 32
    }

    public struct CombatLogEntry
    {
        public string Contents;

        public object Sender;
        public object Target;

        public CombatLogEntryType Type;
    }

    public sealed class CombatLog : Singleton<CombatLog>
    {
        #region Fields
        private readonly List<CombatLogEntry> entries;
        #endregion

        private CombatLog()
                : base()
        {
        }

        public void Log(string contents, CombatLogEntryType type, object sender, object target = null)
        {
            CombatLogEntry entry;

            entry.Contents  = contents;
            entry.Type      = type;
            entry.Sender    = sender;
            entry.Target    = target;

            entries.Add(entry);
        }

        public IEnumerable<CombatLogEntry> Entries()
        {
            for (int i = entries.Count - 1; i > 0; i--) yield return entries[i];
        }

        public void Clear()
        {
            entries.Clear();
        }
    }
}
