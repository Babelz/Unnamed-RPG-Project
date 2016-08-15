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
        public Scene()
        {
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
