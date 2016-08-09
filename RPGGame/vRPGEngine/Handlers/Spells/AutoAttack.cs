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
    public sealed class AutoAttack : SpellHandler
    {
        public AutoAttack() 
            : base("Auto attack", SpellDatabase.Instance.Elements().First(p => p.ID == 9))
        {
        }

        public override void Use(Entity owner)
        {
            var controller = owner.FirstComponentOfType<CharacterController>();

            if (controller == null) return;

            if (controller.TargetFinder.TargetNPC == null) return;

            var pos = controller.TargetFinder.Target.FirstComponentOfType<Transform>().Position;
            var dist = Vector2.Distance(owner.FirstComponentOfType<Transform>().Position, pos);

            // TODO:    melee swings
            //          weapon
            //          spell
            //          damage paska
            //          statuses
        }
        
        public override object Clone()
        {
            return new AutoAttack();
        }
    }
}
