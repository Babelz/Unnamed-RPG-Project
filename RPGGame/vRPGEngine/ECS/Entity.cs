using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace vRPGEngine.ECS
{
    public sealed class Entity : IRegisterEntry
    {
        #region Fields
        private List<IComponent> components;
        
        private List<Entity> childern;
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
        public IEnumerable<Entity> Childern
        {
            get
            {
                return childern;
            }
        }
        #endregion

        internal Entity()
        {
            components = new List<IComponent>();

            childern = new List<Entity>();
        }

        public T AddComponent<T>() where T : Component<T>, new()
        {
            T component = Component<T>.Create(this);

            components.Add(component);

            return component;
        }

        public IEnumerable<IComponent> Components()
        {
            return components;
        }
        public IEnumerable<T> ComponentsOfType<T>() where T : Component<T>, new()
        {
            foreach (var component in components)
            {
                if (component.GetType() == typeof(T)) yield return component as T;
            }
        }
        public T FirstComponentOfType<T>() where T : Component<T>, new()
        {
            var component = components.FirstOrDefault(c => c.GetType() == typeof(T));

            return component != null ? component as T : null;
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

            foreach (var child in childern) child.Destroy();

            childern.Clear();
        }
    }
}
