using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using System.Runtime.CompilerServices;
using vRPGEngine.ECS.Handlers;
using vRPGEngine.Core;

namespace vRPGEngine.ECS.Components
{
    public class ComponentManager<T> : SystemManager<ComponentManager<T>> where T : class, IComponent, new()
    {
        #region Fields
        private readonly RegisterAllocator<T> allocator;

        private IComponentUpdateHanlder<T> handler;

        private List<int> indices;
        private T[] components;
        #endregion

        #region Properties
        public IEnumerable<T> Components
        {
            get
            {
                for (var i = 0; i < indices.Count; i++) yield return components[indices[i]];
            }
        }
        #endregion

        protected ComponentManager()
            : base()
        {
            const int InitialCapacity = 1;

            allocator  = new RegisterAllocator<T>(InitialCapacity, () => { return new T(); });
            components = new T[InitialCapacity];
            indices    = new List<int>();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected override void OnUpdate(GameTime gameTime)
        {
            handler?.Update(this, Components, gameTime);
        }

        public void SetUpdateHandler(IComponentUpdateHanlder<T> handler)
        {
            this.handler = handler;
        }
        public void ClearHandler()
        {
            handler = null;
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
            indices.Add(component.Location);

            return component;
        }
        public void Destroy(T component)
        {
            Debug.Assert(component != null);

            indices.Remove(component.Location);
            allocator.Deallocate(component);

            components[component.Location] = null;
        }
    }
}
