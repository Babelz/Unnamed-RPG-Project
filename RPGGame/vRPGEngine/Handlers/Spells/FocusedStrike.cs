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
            : base(SpellDatabase.Instance.Elements().First(p => p.ID == 10))
        {
        }

        protected override MeleeSpellState OnUse(GameTime gameTime)
        {
            // TODO: fix.
            if (!CanUse()) return MeleeSpellState.Used;

            MeleeSwingResults swing = new MeleeSwingResults();
            UserController.MeleeDamageController.GenerateMeleeAttackPowerBasedSwing(ref swing, 0.15f);

            UserController.TargetFinder.TargetController.Statuses.Health -= swing.Damage;

            SpellHelper.ConsumeCurrencies(UserController.Specialization, UserController.Statuses, Spell);
            GameInfoLog.Instance.LogDealDamage(swing.Damage, swing.Critical, Spell.Name, UserController.TargetFinder.Target.Tags);

            return MeleeSpellState.Used;
        }

        public override object Clone()
        {
            return new FocusedStrike();
        }
    }
}
