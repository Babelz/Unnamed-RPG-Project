using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using vRPGEngine.ECS.Components;

namespace vRPGEngine.ECS.Handlers
{
    public sealed class PlayerCharacterControllerHandler : IComponentUpdateHanlder<PlayerCharacterController>
    {
        public void Update(ComponentManager<PlayerCharacterController> manager, IEnumerable<PlayerCharacterController> components, GameTime gameTime)
        {
            foreach (var component in components) component.Update(gameTime);
        }
    }
}
