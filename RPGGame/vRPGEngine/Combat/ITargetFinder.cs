﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vRPGEngine.ECS;
using vRPGEngine.ECS.Components;

namespace vRPGEngine.Combat
{
    public interface ITargetFinder
    {
        #region Fields
        Entity Target
        {
            get;
        }
        ICharacterController TargetController
        {
            get;
        }
        #endregion

        void ClearTarget();

        void FindTarget(Vector2 position, float radius);
    }
}
