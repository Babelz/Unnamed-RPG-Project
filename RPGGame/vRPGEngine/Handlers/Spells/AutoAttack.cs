using FarseerPhysics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vRPGContent.Data.Spells;
using vRPGEngine.Databases;
using vRPGEngine.ECS;
using vRPGEngine.ECS.Components;

namespace vRPGEngine.Handlers.Spells
{
    public sealed class AutoAttack : MeleeSpellHandler
    {
        public AutoAttack() 
            : base(SpellDatabase.Instance.Elements().First(p => p.ID == 9))
        {
        }
        
        protected override MeleeSpellState OnUse(GameTime gameTime)
        {
            if (UserController.TargetFinder.Target == null)         return MeleeSpellState.Used;
            if (!MeleeHelper.InRange(UserController, User, Spell))  return MeleeSpellState.Used;

            foreach (var swing in UserController.MeleeDamageController.Results())
            {
                var damage = (int)(swing.Damage + UserController.Specialization.TotalMeleePower() * 0.25f);

                UserController.TargetFinder.TargetController.Statuses.Health -= damage;

                GameInfoLog.Instance.LogDealDamage(damage, swing.Critical, Spell.Name, UserController.TargetFinder.Target.Tags);

                if (!UserController.TargetFinder.TargetController.Alive) return MeleeSpellState.Used;
            }

            return MeleeSpellState.Using;
        }

        public override object Clone()
        {
            return new AutoAttack();
        }
    }
}
