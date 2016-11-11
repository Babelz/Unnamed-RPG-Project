using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vRPGEngine.Core;

namespace vRPGEngine.ECS
{
    public sealed class EntityManager : SystemManager<EntityManager>
    {
        #region Fields
        private readonly FreeList<Entity> allocator;

        private Entity[] entitites;
        #endregion

        #region Properties
        public IEnumerable<Entity> Entitites
        {
            get
            {
                foreach (var entity in entitites) if (entity != null) yield return entity;
            }
        }
        public Entity this[int index]
        {
            get
            {
                return entitites[index];
            }
        }
        public int Count
        {
            get
            {
                return entitites.Length;
            }
        }
        #endregion

        private EntityManager()
            : base()
        {
            const int InitialCapacity = 32768;

            allocator = new FreeList<Entity>(InitialCapacity, () => { return new Entity(); });
            entitites = new Entity[InitialCapacity];
        }

        public Entity Create()
        {
            var entity = allocator.Create();

            if (allocator.Size > entitites.Length)
            {
                var newEntitites = new Entity[entitites.Length * 2];

                Array.Copy(entitites, newEntitites, entitites.Length);

                entitites = newEntitites;
            }

            entitites[entity.Location] = entity;

            return entity;
        }
        public void Destroy(Entity entity)
        {
            Debug.Assert(entity != null);

            allocator.Release(entity);

            entitites[entity.Location] = null;
        }

        public void Clear()
        {
            for (var i = 0; i < entitites.Length; i++)
            {
                var entity = entitites[i];

                if (entity != null)
                {
                    allocator.Release(entity);

                    entitites[i] = null;
                }
            }
        }
    }
}
