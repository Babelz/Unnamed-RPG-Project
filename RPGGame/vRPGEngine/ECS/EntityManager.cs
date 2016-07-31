using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vRPGEngine.ECS
{
    public sealed class EntityManager : SystemManager<EntityManager>
    {
        #region Fields
        private readonly RegisterAllocator<Entity> allocator;

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
        #endregion

        private EntityManager()
            : base("entity manager")
        {
            const int InitialCapacity = 1024;

            allocator = new RegisterAllocator<Entity>(InitialCapacity, () => { return new Entity(); });
            entitites = new Entity[InitialCapacity];
        }

        public Entity Create()
        {
            var entity = allocator.Allocate();

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

            allocator.Deallocate(entity);

            entitites[entity.Location] = null;
        }
    }
}
