using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vRPGContent.Data.Spells;
using vRPGEngine.Attributes;
using vRPGEngine.Attributes.Spells;

namespace vRPGEngine
{
    public sealed class CombatLog : Singleton<CombatLog> 
    {
        private CombatLog()
            : base()
        {
        }

        public void LogDamage(Spell spell, MeleeSwingResults damage)
        {
        }
        public void LogBuffGained(Buff buff)
        {
        }
        public void LogStackConsumed(Buff buff)
        {
        }
        public void LogBuffConsumed(Buff buff)
        {
        }
    }
}
