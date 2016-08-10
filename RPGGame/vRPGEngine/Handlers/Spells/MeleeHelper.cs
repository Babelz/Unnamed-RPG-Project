using FarseerPhysics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vRPGContent.Data.Spells;
using vRPGEngine.ECS;
using vRPGEngine.ECS.Components;

namespace vRPGEngine.Handlers.Spells
{
    public static class MeleeHelper
    {
        public static bool InRange(CharacterController controller, Entity sender, Spell spell)
        {
            var pos  = controller.TargetFinder.Target.FirstComponentOfType<Transform>().Position;
            var dist = ConvertUnits.ToSimUnits(Vector2.Distance(sender.FirstComponentOfType<Transform>().Position, pos));

            if (dist > spell.Range) return false;

            return true;
        }
    }
}
