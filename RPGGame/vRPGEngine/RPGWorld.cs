﻿using System;
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
using FarseerPhysics.Common;
using vRPGEngine.Graphics;
using vRPGEngine.Core;

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
            // 60 causes shaking between bodies that collide constantly, use 30 instead.
            // TODO: test.
            const float TimeStep = 1.0f / 30.0f;

            world.Step(TimeStep);
        }

        public Body CreateDynamicBoxCollider(Entity owner, float width, float height)
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
            body.FixedRotation  = true;
            body.UserData       = owner;

            return body;
        }
        public Body CreateKinematicBoxCollider(Entity owner, float width, float height)
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
            body.BodyType       = BodyType.Kinematic;
            body.FixedRotation  = true;
            body.UserData       = owner;

            return body;
        }
        public Body CreateStaticBoxCollider(Entity owner, float width, float height, float x, float y)
        { 
            var body = BodyFactory.CreateRectangle(world,
                                                   ConvertUnits.ToSimUnits(width),
                                                   ConvertUnits.ToSimUnits(height),
                                                   10.0f,
                                                   owner);

            body.Position       = new Vector2(ConvertUnits.ToSimUnits(x), ConvertUnits.ToSimUnits(y));
            body.IsStatic       = true;
            body.Mass           = 500.0f;
            body.Friction       = 0.2f;
            body.Restitution    = 0.2f;
            body.BodyType       = BodyType.Static;
            body.FixedRotation  = true;
            body.UserData       = owner;

            return body;
        }

        public Body CreateStaticPolygonCollider(Entity owner, float x, float y, Vector2[] points)
        {
            var vertices        = new Vertices(points.Select(p => ConvertUnits.ToSimUnits(p)));
            var body            = BodyFactory.CreateLoopShape(world, vertices, owner);

            body.Position       = new Vector2(ConvertUnits.ToSimUnits(x), ConvertUnits.ToSimUnits(y));
            body.IsStatic       = true;
            body.Mass           = 500.0f;
            body.Friction       = 0.2f;
            body.Restitution    = 0.2f;
            body.BodyType       = BodyType.Static;
            body.FixedRotation  = true;
            body.UserData       = owner;
            body.UserData       = owner;

            return body;
        }

        public Body CreateBoxSensor(Entity owner, float width, float height)
        {
            var body = CreateDynamicBoxCollider(owner, width, height);

            body.IsSensor = true;

            return body;
        }
        public Body CreateBoxSensor(Entity owner, float x, float y, float width, float height)
        {
            var body = CreateBoxSensor(owner, width, height);

            body.Position = ConvertUnits.ToSimUnits(x, y);

            return body;
        }

        public Body CreateBoxSensor(Entity owner, Vector2 position, float width, float height)
        {
            return CreateBoxSensor(owner, position.X, position.Y, width, height);
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
