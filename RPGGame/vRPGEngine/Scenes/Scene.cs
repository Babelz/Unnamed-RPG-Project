using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vRPGEngine.ECS;

namespace vRPGEngine.Scenes
{
    public abstract class Scene
    {
        #region Fields
        private readonly List<Entity> entitites;
        #endregion

        #region Properties
        protected IEnumerable<Entity> Entitites
        {
            get
            {
                return entitites;
            }
        }
        #endregion

        public Scene()
        {
            entitites = new List<Entity>();
        }

        protected void Register(Entity entity)
        {
            entitites.Add(entity);
        }
        protected void Unregister(Entity entity)
        {
            entitites.Add(entity);
        }

        public virtual void Initialize()
        {
        }
        public virtual void Deinitialize()
        {
        }
        
        public virtual bool ChangeScene()
        {
            return true;
        }

        public virtual void Update(GameTime gameTime)
        {
        }
    }
}
