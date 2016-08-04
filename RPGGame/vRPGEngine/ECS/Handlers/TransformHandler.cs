using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using vRPGEngine.ECS.Components;
using FarseerPhysics;

namespace vRPGEngine.ECS.Handlers
{
    public sealed class TransformHandler : IComponentUpdateHanlder<Transform>
    {
        public void Update(ComponentManager<Transform> manager, IEnumerable<Transform> components, GameTime gameTime)
        {
            foreach (var transform in components)
            {
                var body = transform.Owner.FirstComponentOfType<BoxCollider>();

                if (body == null) continue;

                transform.Position = ConvertUnits.ToDisplayUnits(body.Position);
                transform.Size     = ConvertUnits.ToDisplayUnits(body.Size);
            }
        }
    }
}
