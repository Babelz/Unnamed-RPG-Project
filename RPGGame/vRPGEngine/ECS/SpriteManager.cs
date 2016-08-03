using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using vRPGEngine.ECS.Components;

namespace vRPGEngine.ECS
{
    public sealed class SpriteManager : ComponentManager<SpriteRenderer>
    {
        private SpriteManager()
            : base()
        {
        }
        
        protected override void OnUpdate(GameTime gameTime)
        {
            foreach (var renderer in Components)
            {
                if (!renderer.Anchored) continue;

                var transform = renderer.Owner.FirstComponentOfType<Transform>();

                if (transform == null) continue;

                renderer.Sprite.Position = renderer.Sprite.Position + transform.Position;
            }
        }
    }
}
