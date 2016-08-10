using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using vRPGEngine.ECS.Components;

namespace vRPGEngine.ECS.Handlers
{
    public sealed class CharacterControllerHandler : IComponentUpdateHanlder<CharacterController>
    {
        public void Update(ComponentManager<CharacterController> manager, IEnumerable<CharacterController> components, GameTime gameTime)
        {
            foreach (var component in components) component.Update(gameTime);
        }
    }
}
