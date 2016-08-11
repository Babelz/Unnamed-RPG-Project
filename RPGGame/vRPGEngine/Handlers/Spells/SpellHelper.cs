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
        public static int ComputeCost(Specialization specialization, Spell spell)
        {
            var cost = spell.Cost * 0.1f;

            switch (spell.CostType)
            {
                case SpellCostType.None:            return 0;
                case SpellCostType.BasePercent:     return (int)(GetCurrencyFor(specialization, spell, true) * cost);
                case SpellCostType.TotalPercent:    return (int)(GetCurrencyFor(specialization, spell, false) * cost);
                case SpellCostType.Static:          return spell.Cost;
                default:                            throw new InvalidOperationException("invalid spell cost type");
            }
        }

        public static int GetCurrencyFor(Specialization specialization, Spell spell, bool baseValue = true)
        {
            switch (spell.Consumes)
            {
                case Currencies.Mana:   return baseValue ? specialization.BaseMana()    : specialization.TotalMana();
                case Currencies.Focus:  return baseValue ? specialization.BaseFocus()   : specialization.TotalFocus();
                case Currencies.Health: return baseValue ? specialization.TotalHealth() : specialization.TotalHealth();
                default:                throw new InvalidOperationException("invalid currency type");
            }
        }

        public static void ConsumeCurrencies(Specialization specialization, Statuses statuses, Spell spell)
        {
            var cost = ComputeCost(specialization, spell);

            switch (spell.Consumes)
            {
                case Currencies.Mana:   statuses.Mana -= cost;      break;
                case Currencies.Focus:  statuses.Focus -= cost;     break;
                case Currencies.Health: statuses.Health -= cost;    break;
                default: break;
            }
        }

        public static bool CanUse(Specialization specialization, Statuses statuses, Spell spell)
        {
            var cost = ComputeCost(specialization, spell);

            switch (spell.Consumes)
            {
                case Currencies.Mana:   return statuses.Mana >= cost;      
                case Currencies.Focus:  return statuses.Focus >= cost;     
                case Currencies.Health: return statuses.Health >= cost;    
                default:                return true;
            }
        }
    }
}
