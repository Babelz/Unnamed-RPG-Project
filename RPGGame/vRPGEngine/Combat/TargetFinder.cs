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
    public sealed class TargetFinder : ITargetFinder
    {
        #region Properties
        public Entity Target
        {
            get;
            set;
        }
        public ICharacterController TargetController
        {
            get;
            set;
        }
        #endregion

        #region Events
        public event TargetFinderEventHandler TargetChanged;
        #endregion

        public TargetFinder()
        {
        }

        public void ClearTarget()
        {
            Target           = null;
            TargetController = null;

            TargetChanged?.Invoke(this);
        }

        public void FindTarget(Vector2 position, float radius)
        {
            //return;
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

            TargetController = Target.FirstComponentOfType<ICharacterController>();

            // Clear target, not an NPC.
            if (TargetController == null)
            {
                ClearTarget();
                return;
            }
            
            TargetChanged?.Invoke(this);
        }
    }
}
