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
            : base("auto attack")
        {
        }
        
        protected override MeleeSpellState OnUse(GameTime gameTime)
        {
            if (UserController.TargetFinder.Target == null)         return MeleeSpellState.Active;
            if (!MeleeHelper.InRange(UserController, User, Spell))  return MeleeSpellState.Active;

            foreach (var swing in UserController.MeleeDamageController.Results())
            {
                var damage = (int)(swing.Damage + UserController.Specialization.TotalMeleePower() * 0.25f);

                UserController.TargetFinder.TargetController.Statuses.Health -= damage;

                GameInfoLog.Instance.LogDealDamage(damage, swing.Critical, Spell.Name, UserController.TargetFinder.TargetController.Name);

                if (!UserController.TargetFinder.TargetController.Statuses.Alive) return MeleeSpellState.Active;
            }

            return MeleeSpellState.Inactive;
        }

        public override object Clone()
        {
            return new AutoAttack();
        }
    }
}
