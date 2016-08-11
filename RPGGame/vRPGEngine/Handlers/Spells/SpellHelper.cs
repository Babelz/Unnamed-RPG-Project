using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vRPGContent.Data.Attributes;
using vRPGContent.Data.Spells;
using vRPGEngine.Attributes;
using vRPGEngine.Specializations;

namespace vRPGEngine.Handlers.Spells
{
    public static class SpellHelper
    {
        public static int ComputeCost(Specialization specialization, Statuses statuses, Spell spell)
        {
            var cost = spell.Cost * 0.1f;

            switch (spell.CostType)
            {
                case SpellCostType.None:
                    return 0;
                case SpellCostType.BasePercent:
                    return (int)(GetCurrencyFor(specialization, spell) * cost);
                case SpellCostType.TotalPercent:
                    break;
                case SpellCostType.Static:
                    break;
                default:
                    throw new InvalidOperationException("invalid spell cost type");
            }
        }

        public static int GetCurrencyFor(Specialization specialization, Spell spell)
        {
            switch (spell.Consumes)
            {
                case Currencies.Mana:   return specialization.TotalMana();
                case Currencies.Focus:  return specialization.TotalFocus();
                case Currencies.Health: return specialization.TotalHealth();
                default:                throw new InvalidOperationException("invalid currency type");
            }
        }
    }
}
