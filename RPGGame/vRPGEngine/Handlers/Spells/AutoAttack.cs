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

        public override void Update(GameTime gameTime)
        {
            //if (!Working) return;
            
            //controller.MeleeDamageController.Tick(gameTime);

            //if (controller.TargetFinder.Target == null) return;

            //if (!MeleeHelper.InRange(controller, owner, Spell)) return;

            //foreach (var swing in controller.MeleeDamageController.Results())
            //{
            //    var damage = (int)(swing.Damage + controller.Specialization.TotalMeleePower() * 0.25f);

            //    controller.TargetFinder.TargetNPC.Handler.Data.Health -= damage;

            //    GameInfoLog.Instance.Log(damage.ToString(), InfoLogEntryType.Message);

            //    if (!controller.TargetFinder.TargetNPC.Alive)
            //    {
            //        controller.TargetFinder.ClearTarget();

            //        controller.MeleeDamageController.LeaveCombat();

            //        Working = false;
            //    }
            //}
        }
        
        public override object Clone()
        {
            return new AutoAttack();
        }
    }
}
