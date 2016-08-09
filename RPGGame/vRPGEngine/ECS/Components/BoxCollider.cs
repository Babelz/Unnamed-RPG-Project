using FarseerPhysics;
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
        public Vector2 SimulationPosition
        {
            get
            {
                return body.Position;
            }
            set
            {
                var simPosition = ConvertUnits.ToSimUnits(value);
                body.Position   = simPosition;
            }
        }
        public Vector2 DisplayPosition
        {
            get
            {
                return ConvertUnits.ToDisplayUnits(SimulationPosition);
            }
        }
        public Vector2 SimulationSize
        {
            get
            {
                AABB aabb = new AABB();

                body.FixtureList.First().GetAABB(out aabb, 0);

                return new Vector2(aabb.Width, aabb.Height);
            }
        }
        public Category Category
        {
            set
            {
                body.CollisionCategories = value;
            }
        }
        public Category CollidesWith
        {
            set
            {
                body.CollidesWith = value;
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
        
        public void MakeDynamic(float width, float height)
        {
            if (body != null) RPGWorld.Instance.DestroyBody(body);

            body = RPGWorld.Instance.CreateDynamicCollider(Owner, width, height);
        }

        public void MakeStatic(float width, float height, float x, float y)
        {
            if (body != null) RPGWorld.Instance.DestroyBody(body);

            body = RPGWorld.Instance.CreateStaticCollider(Owner, width, height, x + width / 2.0f, y + height / 2.0f);
        }

        public void MakeKinematic(float width, float height)
        {
            if (body != null) RPGWorld.Instance.DestroyBody(body);

            body = RPGWorld.Instance.CreateKinematicCollider(Owner, width, height);
        }
    }
}
