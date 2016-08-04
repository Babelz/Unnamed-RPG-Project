using FarseerPhysics.Dynamics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vRPGEngine.ECS.Components;

namespace vRPGEngine.ECS.Components
{
    public sealed class BoxCollider : Component<BoxCollider>
    {
        #region Fields
        private Body body;
        #endregion

        public BoxCollider()
            : base()
        {
        }
        
        public void Construct(float with, float height)
        {
            if (body != null)
            {
            }
        }
    }
}
