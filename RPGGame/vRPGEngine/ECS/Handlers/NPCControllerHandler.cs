using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using vRPGEngine.ECS.Components;

namespace vRPGEngine.ECS.Handlers
{
    public sealed class NPCControllerHandler : IComponentUpdateHanlder<NPCController>
    {
        public void Update(ComponentManager<NPCController> manager, IEnumerable<NPCController> components, GameTime gameTime)
        {
            foreach (var component in components) component.Update(gameTime);
        }
    }
}
