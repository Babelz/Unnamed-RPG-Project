﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vRPGEngine.ECS
{
    public class ComponentManager<T> : SystemManager<ComponentManager<T>> where T : class, IComponent, new()
    {
        #region Fields
        private readonly RegisterAllocator<T> allocator;

        private T[] components;
        #endregion

        #region Properties
        protected IEnumerable<T> Components
        {
            get
            {
                foreach (var component in components) if (component != null) yield return component;
            }
        }
        #endregion

        protected ComponentManager()
            : base("component manager: " + typeof(T).Name)
        {
            const int InitialCapacity = 128;

            allocator  = new RegisterAllocator<T>(InitialCapacity, () => { return new T(); });
            components = new T[InitialCapacity];
        }

        protected override void OnActivate()
        {
            SystemManagers.Instance.Register(this);
        }
        protected override void OnSuspend()
        {
            SystemManagers.Instance.Unregister(this);
        }

        public T Create()
        {
            var component = allocator.Allocate();

            if (allocator.Size > components.Length)
            {
                var newComponents = new T[allocator.Size];

                Array.Copy(components, newComponents, components.Length);

                components = newComponents;
            }

            components[component.Location] = component;

            return component;
        }
        public void Destroy(T component)
        {
            Debug.Assert(component != null);

            allocator.Deallocate(component);

            components[component.Location] = null;
        }
    }
}
