using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using vRPGContent.Data.Spells;
using vRPGEngine.Databases;

namespace vRPGEngine.Handlers.Spells
{
    public sealed class FocusedStrike : MeleeSpellHandler
    {
        public FocusedStrike() 
            : base(SpellDatabase.Instance.Elements().First(p => p.ID == 10))
        {
        }

        protected override bool Tick(GameTime gameTime)
        {
            // TODO: fix.
            if (UserController.TargetFinder.Target == null)                                         return false;
            if (!MeleeHelper.InRange(UserController, User, Spell))                                  return false;
            if (!SpellHelper.CanUse(UserController.Specialization, UserController.Statuses, Spell)) return false;

            var damage = (int)(UserController.Specialization.TotalMeleePower() * 0.15f);

            UserController.TargetFinder.TargetController.Statuses.Health -= damage;

            SpellHelper.ConsumeCurrencies(UserController.Specialization, UserController.Statuses, Spell);
            GameInfoLog.Instance.Log(damage.ToString(), InfoLogEntryType.Message);

            return false;
        }

        public override object Clone()
        {
            return new FocusedStrike();
        }
    }
}
