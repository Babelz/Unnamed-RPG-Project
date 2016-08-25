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
    public enum ColliderType
    {
        None,
        Box,
        Polygon
    }

    public sealed class Collider : Component<Collider>
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
                body.Position = ConvertUnits.ToSimUnits(value);
            }
        }
        public Vector2 DisplayPosition
        {
            get
            {
                return ConvertUnits.ToDisplayUnits(SimulationPosition);
            }
            set
            {
                body.Position = ConvertUnits.ToSimUnits(value);
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
        public ColliderType Type
        {
            get;
            private set;
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

        public Collider()
            : base()
        {
            body = null;
        }
        
        public void MakeDynamicBox(float width, float height)
        {
            if (body != null) RPGWorld.Instance.DestroyBody(body);

            body = RPGWorld.Instance.CreateDynamicBoxCollider(Owner, width, height);

            Type = ColliderType.Box;
        }

        public void MakeStaticBox(float width, float height, float x, float y)
        {
            if (body != null) RPGWorld.Instance.DestroyBody(body);

            body = RPGWorld.Instance.CreateStaticBoxCollider(Owner, width, height, x + width / 2.0f, y + height / 2.0f);

            Type = ColliderType.Box;
        }

        public void MakeKinematicBox(float width, float height)
        {
            if (body != null) RPGWorld.Instance.DestroyBody(body);

            body = RPGWorld.Instance.CreateKinematicBoxCollider(Owner, width, height);

            Type = ColliderType.Box;
        }

        public void MakeStaticPolygon(float x, float y, Vector2[] points)
        {
            if (body != null) RPGWorld.Instance.DestroyBody(body);

            body = RPGWorld.Instance.CreateStaticPolygonCollider(Owner, x, y, points);

            Type = ColliderType.Polygon;
        }
    }
}
