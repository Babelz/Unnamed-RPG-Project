using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vRPGEngine.ECS
{
    public sealed class EntityBuilder : Singleton<EntityBuilder>
    {
        #region Fields
        private Dictionary<string, Func<Entity>> prefabs;
        #endregion

        private EntityBuilder()
            : base()
        {
            prefabs = new Dictionary<string, Func<Entity>>();
        }

        public Entity Create(string name)
        {
            Debug.Assert(!string.IsNullOrEmpty(name));

            return prefabs[name]();
        }

        public void RegisterPrefab(string name, Func<Entity> activator)
        {
            Debug.Assert(!string.IsNullOrEmpty(name));
            Debug.Assert(activator != null);

            prefabs.Add(name, activator);
        }
        public void UnregisterPrefab(string name)
        {
            Debug.Assert(!string.IsNullOrEmpty(name));

            prefabs.Remove(name);
        }
    }
}
