using FarseerPhysics.Collision;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        #region Properties
        public bool Active
        {
            get
            {
                return body != null;
            }
        }
        public Vector2 LinearVelocity
        {
            get
            {
                Debug.Assert(body != null);

                return body.LinearVelocity;
            }
            set
            {
                Debug.Assert(body != null);

                body.LinearVelocity = value;
            }
        }
        public Vector2 Position
        {
            get
            {
                return body.Position;
            }
        }
        public Vector2 Size
        {
            get
            {
                AABB aabb = new AABB();

                body.FixtureList.First().GetAABB(out aabb, 0);

                return new Vector2(aabb.Width, aabb.Height);
            }
        }
        #endregion

        #region Events
        public event OnCollisionEventHandler OnCollision
        {
            add
            {
                Debug.Assert(body != null);

                body.OnCollision += value;
            }
            remove
            {
                Debug.Assert(body != null);

                body.OnCollision -= value;
            }
        }
        public event OnSeparationEventHandler OnSeparation
        {
            add
            {
                Debug.Assert(body != null);

                body.OnSeparation += value;
            }
            remove
            {
                Debug.Assert(body != null);

                body.OnSeparation -= value;
            }
        }
        #endregion

        public BoxCollider()
            : base()
        {
            body = null;
        }
        
        public void Initialize(float width, float height)
        {
            if (body != null) RPGWorld.Instance.DestroyBody(body);

            body = RPGWorld.Instance.CreateEntityCollider(Owner, width, height);
        }
    }
}
