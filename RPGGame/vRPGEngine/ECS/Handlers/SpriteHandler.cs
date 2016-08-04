using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using vRPGEngine.ECS.Components;

namespace vRPGEngine.ECS.Handlers
{
    public sealed class SpriteHandler : IComponentUpdateHanlder<SpriteRenderer>
    {
        public void Update(ComponentManager<SpriteRenderer> manager, IEnumerable<SpriteRenderer> components, GameTime gameTime)
        {
            foreach (var renderer in components)
            {
                if (!renderer.Anchored) continue;

                var transform = renderer.Owner.FirstComponentOfType<Transform>();

                if (transform == null) continue;

                renderer.Sprite.Position = transform.Position + renderer.PositionOffset;
            }
        }
    }
}
