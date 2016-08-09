using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using vRPGEngine.ECS.Components;

namespace vRPGEngine.ECS.Handlers
{
    public sealed class SpawnAreaHandler : IComponentUpdateHanlder<SpawnArea>
    {
        public void Update(ComponentManager<SpawnArea> manager, IEnumerable<SpawnArea> components, GameTime gameTime)
        {
            foreach (var area in components) area.Update(gameTime);
        }
    }
}
