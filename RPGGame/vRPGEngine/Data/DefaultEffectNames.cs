using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vRPGEngine.Data
{
    public static class DefaultEffectNames
    {
        public static readonly string BuffStats         = "buff_stats";
        public static readonly string RestoreMana       = "restore_mana";
        public static readonly string RestoreFocus      = "restore_focus";
        public static readonly string RestoreHealth     = "restore_health";

        private static readonly string[] DefaultEffects = new string[]
        {
            BuffStats,
            RestoreMana,
            RestoreFocus,
            RestoreHealth
        };

        public static bool UsesDefaultEffect(Consumable item)
        {
            return DefaultEffects.Contains(item.EffectName);
        }
    }
}
