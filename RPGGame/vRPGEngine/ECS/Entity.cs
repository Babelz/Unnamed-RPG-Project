using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using vRPGEngine.ECS.Components;

namespace vRPGEngine.ECS
{
    public sealed class Entity : IRegisterEntry
    {
        #region Fields
        private List<IComponent> components;
        
        private List<Entity> children;
        #endregion

        #region Properties
        public int Location
        {
            get;
            set;
        }
        public string Tags
        {
            get;
            set;
        }
        public IEnumerable<Entity> Children
        {
            get
            {
                return children;
            }
        }
        #endregion

        internal Entity()
        {
            components  = new List<IComponent>();
            children    = new List<Entity>();
        }

        public void AddChildren(Entity child)
        {
            Debug.Assert(child != null);

            if (children.Contains(child)) return;

            children.Add(child);
        }
        public bool RemoveChildren(Entity child)
        {
            return children.Remove(child);
        }

        public T AddComponent<T>() where T : Component<T>, IComponent, new()
        {
            T component = Component<T>.Create(this) as T;

            components.Add(component);

            return component;
        }

        public IEnumerable<IComponent> Components()
        {
            return components;
        }
        public IEnumerable<T> ComponentsOfType<T>() where T : class, IComponent
        {
            if (typeof(T).IsInterface)
            {
                foreach (var component in components)
                {
                    var interfaceReference = component as T;

                    if (interfaceReference != null) yield return interfaceReference;
                }
            }
            else
            {
                foreach (var component in components) if (component.GetType() == typeof(T)) yield return component as T;
            }
        }
        public T FirstComponentOfType<T>() where T : class, IComponent
        {
            if (typeof(T).IsInterface)
            {
                foreach (var component in components)
                {
                    var interfaceReference = component as T;

                    if (interfaceReference != null) return interfaceReference;
                }
            }
            else
            {
                var results = components.FirstOrDefault(c => c.GetType() == typeof(T));

                return results != null ? results as T : null;
            }

            return null;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Entity Create()
        {
            var entity = EntityManager.Instance.Create();

            entity.Tags = string.Empty;

            return entity;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Destroy()
        {
            foreach (var component in components) component.Destroy();

            EntityManager.Instance.Destroy(this);

            foreach (var child in children) child.Destroy();

            children.Clear();
        }
    }
}
