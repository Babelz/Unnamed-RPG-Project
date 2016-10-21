using FarseerPhysics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vRPGContent.Data.Spells;
using vRPGEngine;
using vRPGEngine.Databases;
using vRPGEngine.ECS;
using vRPGEngine.ECS.Components;
using vRPGEngine.Handlers.Spells;

namespace RPGGame.Handlers.Spells
{
    //public sealed class AutoAttack : MeleeSpellHandler
    //{
    //    public AutoAttack() 
    //        : base("auto attack")
    //    {
    //    }
        
    //    protected override MeleeSpellState OnUse(GameTime gameTime)
    //    {
    //        if (UserController.TargetFinder.Target == null)         return MeleeSpellState.Inactive;
    //        if (!SpellHelper.InRange(UserController, User, Spell))  return MeleeSpellState.Inactive;

    //        foreach (var swing in UserController.MeleeDamageController.Results())
    //        {
    //            var damage = (int)(swing.Damage + UserController.Specialization.TotalAttackPower() * 0.25f);

    //            UserController.TargetFinder.TargetController.Statuses.Health -= damage;

    //            GameInfoLog.Instance.LogDealDamage(damage, swing.Critical, Spell.Name, UserController.TargetFinder.TargetController.Name);

    //            if (!UserController.TargetFinder.TargetController.Statuses.Alive) return MeleeSpellState.Inactive;
    //        }

    //        return MeleeSpellState.Active;
    //    }

    //    public override SpellHandler Clone()
    //    {
    //        return new AutoAttack();
    //    }
    //}
}
