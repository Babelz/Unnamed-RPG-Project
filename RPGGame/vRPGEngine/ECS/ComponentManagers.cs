﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vRPGEngine.ECS
{
    public sealed class ComponentManagers : Singleton<ComponentManagers>
    {
        #region Fields
        private List<ISystemManager> managers;
        #endregion

        #region Properties
        public IEnumerable<ISystemManager> Managers
        {
            get
            {
                return managers;
            }
        }
        #endregion

        public ComponentManagers() 
            : base()
        {
            managers = new List<ISystemManager>();
        }

        public void Register(ISystemManager manager)
        {
            Debug.Assert(manager != null);

            for (var i = 0; i < managers.Count; i++)
            {
                if (ReferenceEquals(managers[i], manager)) throw new vRPGEngineException("attempting to register single system twice");
            }

            managers.Add(manager);
        }
        public void Unregister(ISystemManager manager)
        {
            Debug.Assert(manager != null);

            managers.Remove(manager);
        }

        public void Update(GameTime gameTime)
        {
            for (var i = 0; i < managers.Count; i++) managers[i].Update(gameTime);
        }
    }
}
