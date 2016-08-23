using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using vRPGEngine.ECS.Components;
using vRPGEngine.Graphics;

namespace vRPGEngine.ECS.Handlers
{
    public sealed class SpriteHandler : IComponentUpdateHanlder<SpriteRenderer>
    {
        public void Update(ComponentManager<SpriteRenderer> manager, IEnumerable<SpriteRenderer> components, GameTime gameTime)
        {
            foreach (var renderer in components)
            {
                if (renderer.Flags == RenderFlags.None) continue;

                var transform = renderer.Owner.FirstComponentOfType<Transform>();

                if (transform == null) continue;

                if (renderer.Flags.HasFlag(RenderFlags.Anchored)) renderer.Sprite.Position = transform.Position + renderer.PositionOffset;

                if (renderer.Flags.HasFlag(RenderFlags.AutomaticDepth))
                {
                    var depth = Math.Abs((transform.Position.Y - transform.Size.Y * transform.Scale.Y) / Renderer.Instance.LayerHeight);

                    renderer.Sprite.Depth = depth;
                }
            }
        }
    }
}
