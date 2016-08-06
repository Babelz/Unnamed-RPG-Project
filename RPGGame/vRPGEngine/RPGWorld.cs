using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using FarseerPhysics;
using System.Diagnostics;
using vRPGEngine.ECS;
using FarseerPhysics.Collision;

namespace vRPGEngine
{
    public sealed class RPGWorld : SystemManager<RPGWorld>
    {
        #region Constants
        public const float UnitToPixel = 32.0f;
        public const float PixelToUnit = 1.0f / UnitToPixel;
        #endregion

        #region Fields
        private readonly World world;        
        #endregion

        private RPGWorld()
            : base()
        {
            world = new World(new Vector2(0.0f, 0.0f));
        }

        protected override void OnUpdate(GameTime gameTime)
        {
            const float TimeStep = 1.0f / 60.0f;

            world.Step(TimeStep);
        }

        public Body CreateEntityCollider(Entity owner, float width, float height)
        {
            var body = BodyFactory.CreateRectangle(world,
                                                   ConvertUnits.ToSimUnits(width),
                                                   ConvertUnits.ToSimUnits(height),
                                                   10.0f,
                                                   owner);
            body.IsStatic       = false;
            body.Mass           = 80.0f;
            body.Friction       = 0.2f;
            body.Restitution    = 0.2f;
            body.BodyType       = BodyType.Dynamic;
            
            return body;
        }
        public Body CreateSensor(Entity owner, float width, float height)
        {
            var body = CreateEntityCollider(owner, width, height);

            body.IsSensor = true;

            return body;
        }

        public void DestroyBody(Body body)
        {
            Debug.Assert(body != null);

            world.RemoveBody(body);
        }

        public IEnumerable<Body> QueryArea(Vector2 simPosition, float simRadius)
        {
            var aabb = new AABB(simPosition, simRadius, simRadius);

            return world.QueryAABB(ref aabb).Select(f => f.Body);
        }
    }
}
