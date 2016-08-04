using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using vRPGEngine.ECS.Components;

namespace vRPGEngine.ECS.Handlers
{
    public sealed class BehaviourHandler : IComponentUpdateHanlder<Behaviour>
    {
        public void Update(ComponentManager<Behaviour> manager, IEnumerable<Behaviour> components, GameTime gameTime)
        {
            foreach (var behaviour in components) behaviour.Update(gameTime);
        }
    }
}
