using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace vRPGEngine.ECS.Components
{
    public class Component<T> : IComponent where T : Component<T>, new()
    {
        #region Properties
        public Entity Owner
        {
            get;
            private set;
        }
        public int Location
        {
            get;
            set;
        }
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

            var component   = ComponentManager<T>.Instance.Create();
            component.Owner = owner;

            component.Initialize();

            return component;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Destroy()
        {
            // TODO: do performance tests. As vs (T) syntax.
            //       (T) syntax can be compile time and i am
            //       pretty sure it will be with generics.
            ComponentManager<T>.Instance.Destroy(this as T);
            
            Deinitialize();
            
            Owner = null;
        }
    }
}
