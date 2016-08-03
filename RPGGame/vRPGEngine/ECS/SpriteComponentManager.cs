using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using vRPGEngine.ECS.Components;

namespace vRPGEngine.ECS
{
    public sealed class SpriteComponentManager : ComponentManager<SpriteRenderer>
    {
        private SpriteComponentManager()
            : base()
        {
        }
        
        protected override void OnUpdate(GameTime gameTime)
        {
            foreach (var renderer in Components)
            {
                var transform = renderer.Owner.FirstComponentOfType<Transform>();

                if (transform != null)
                {
                }
            }
        }
    }
}
