using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vRPGEngine.Specializations;

namespace vRPGEngine.Attributes
{
    public enum PowerSource : int
    {
        SpellPower,
        AttackPower,
        Agility,
        AttackPowerWithAgility
    }

    /// <summary>
    /// Class that is responsible of handling ranged spell
    /// handling.
    /// </summary>
    public sealed class RangedSpellController
    {
        public RangedSpellController()
        {
        }

        private int GetPower(PowerSource source, Specialization specialization)
        {
            switch (source)
            {
                case PowerSource.SpellPower:
                    return specialization
                    break;
                case PowerSource.AttackPower:
                    break;
                case PowerSource.Agility:
                    break;
                case PowerSource.AttackPowerWithAgility:
                    break;
                default:
                    break;
            }
        }
    }
}
