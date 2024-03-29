﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace vRPGEngine.ECS.Components
{
    public class Component<T> : IComponent where T : Component<T>, IComponent, new()
    {
        #region Properties
        public Entity Owner
        {
            get;
            set;
        }
        public int Location
        {
            get;
            set;
        }
        #endregion

        #region Events 
        public event ComponentEventHandler Destroyed;
        #endregion

        public Component()
        {
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected virtual void Initialize()
        {
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected virtual void Deinitialize()
        {
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T Create(Entity owner)
        {
            Debug.Assert(owner != null);

            var component           = ComponentManager<T>.Instance.Create();
            component.Owner         = owner;

            component.Initialize();
            
            return component as T;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Destroy()
        {
            ComponentManager<T>.Instance.Destroy((T)this);

            Destroyed?.Invoke(this);

            Deinitialize();
            
            Owner = null;
        }
    }
}
