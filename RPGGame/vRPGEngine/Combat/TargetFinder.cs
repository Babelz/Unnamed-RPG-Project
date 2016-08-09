using FarseerPhysics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vRPGEngine.ECS;
using vRPGEngine.ECS.Components;

namespace vRPGEngine.Combat
{
    public sealed class TargetFinder
    {
        #region Properties
        public Entity Target
        {
            get;
            set;
        }
        public NPCController TargetNPC
        {
            get;
            set;
        }
        #endregion

        public TargetFinder()
        {
        }

        public void ClearTarget()
        {
            Target      = null;
            TargetNPC   = null;
        }

        public void FindTarget(Vector2 position, float radius)
        {
            var entitites   = RPGWorld.Instance.QueryArea(ConvertUnits.ToSimUnits(position), ConvertUnits.ToSimUnits(radius));

            if (entitites.Count() == 0)
            {
                ClearTarget();
                return;
            }

            var data        = entitites.FirstOrDefault().UserData;

            if (data == null)
            {
                ClearTarget();
                return;
            }

            Target          = data as Entity;

            if (Target == null)
            {
                ClearTarget();
                return;
            }

            TargetNPC       = Target.FirstComponentOfType<NPCController>();

            // Clear target, not an NPC.
            if (TargetNPC == null)
            {
                ClearTarget();
                return;
            }
        }
    }
}
