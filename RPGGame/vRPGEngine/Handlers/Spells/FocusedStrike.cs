using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using vRPGContent.Data.Spells;
using vRPGEngine.Databases;
using vRPGEngine.Attributes;

namespace vRPGEngine.Handlers.Spells
{
    public sealed class FocusedStrike : MeleeSpellHandler
    {
        public FocusedStrike() 
            : base("focused strike")
        {
        }

        protected override MeleeSpellState OnUse(GameTime gameTime)
        {
            // TODO: fix.
            if (!CanUse()) return MeleeSpellState.Active;

            MeleeSwingResults swing = new MeleeSwingResults();
            UserController.MeleeDamageController.GenerateSwing(ref swing, 0.15f);

            UserController.TargetFinder.TargetController.Statuses.Health -= swing.Damage;

            SpellHelper.ConsumeCurrencies(UserController.Specialization, UserController.Statuses, Spell);
            GameInfoLog.Instance.LogDealDamage(swing.Damage, swing.Critical, Spell.Name, UserController.TargetFinder.TargetController.Name);

            return MeleeSpellState.Active;
        }

        public override object Clone()
        {
            return new FocusedStrike();
        }
    }
}
