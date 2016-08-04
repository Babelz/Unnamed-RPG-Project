using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using vRPGEngine.ECS;
using vRPGEngine.ECS.Components;

namespace vRPGEngine.ECS
{
    public sealed class BehaviourManager : ComponentManager<Behaviour>
    {
        private BehaviourManager()
            : base()
        {
        }

        protected override void OnUpdate(GameTime gameTime)
        {
            foreach (var behaviour in Components) behaviour.Update(gameTime);
        }
    }
}
