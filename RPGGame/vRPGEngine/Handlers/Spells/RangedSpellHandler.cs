using FarseerPhysics;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Contacts;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vRPGEngine.Attributes;
using vRPGEngine.Databases;
using vRPGEngine.ECS.Components;

namespace vRPGEngine.Handlers.Spells
{
    public abstract class RangedSpellHandler : SpellHandler
    {
        #region Constants
        private const float SpellHitboxBounds = 32.0f;
        #endregion

        #region Properties
        protected Body Sensor
        {
            get;
            private set;
        }

        protected PowerSource Source
        {
            get;
            private set;
        }
        protected float PercentageOfSource
        {
            get;
            private set;
        }
        protected int BaseDamage
        {
            get;
            private set;
        }

        protected RangedDamageResults LastResults
        {
            get;
            private set;
        }
        #endregion

        public RangedSpellHandler(string name, PowerSource source, float percentageOfSource, int baseDamage)
            : base(SpellDatabase.Instance.First(e => e.Name.ToLower() == name))
        {
            Source              = source;
            PercentageOfSource  = percentageOfSource;
            BaseDamage          = baseDamage;
        }

        #region Event handlers
        private void Controller_OnEndCast(bool interrupted)
        {
            if (interrupted) UnregisterEvents();
        }
        private void Controller_CastSuccessful(ref RangedDamageResults results)
        {
            LastResults = results;

            CreateSensor();
            SendMissile();
        }
        private bool Sensor_OnCollision(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            if (ReferenceEquals(fixtureB.Body.UserData, Target))
            {
                DealDamage();

                UnregisterEvents();
                DisposeSensor();

                return true;
            }

            return false;
        }
        #endregion

        private void RegisterEvents()
        {
            var controller              = Controller.RangedDamageController;

            controller.CastSuccessful   += Controller_CastSuccessful;
            controller.OnEndCast        += Controller_OnEndCast;
        }
        private void UnregisterEvents()
        {
            var controller              = Controller.RangedDamageController;

            controller.CastSuccessful   -= Controller_CastSuccessful;
            controller.OnEndCast        -= Controller_OnEndCast;
        }

        private void CreateSensor()
        {
            var position                = User.FirstComponentOfType<Transform>().Position;

            Sensor                      = RPGWorld.Instance.CreateBoxSensor(User, position, SpellHitboxBounds, SpellHitboxBounds);
            Sensor.CollidesWith         = Category.All;
            Sensor.CollisionCategories  = Category.All;
            Sensor.OnCollision          += Sensor_OnCollision;        
        }

        private void DisposeSensor()
        {
            Sensor.OnCollision -= Sensor_OnCollision;

            RPGWorld.Instance.DestroyBody(Sensor);
        }

        protected override bool CanUse()
        {
            if (Controller.RangedDamageController.Casting)
            {
                if (UserIsPlayer) GameInfoLog.Instance.LogWarning("already casting a spell!");

                return false;
            }

            return base.CanUse();
        }

        protected override void Use()
        {
            Debug.Assert(Controller.RangedDamageController != null);
            
            RegisterEvents();

            Controller.RangedDamageController.BeginCast(Spell, Source, PercentageOfSource, BaseDamage);
        }

        protected virtual void SendMissile()
        {
        }
    }

    public abstract class BasicMissileSpellHandler : RangedSpellHandler
    {
        #region Constant fields
        private const float MissileVelocity = 1.32f;
        #endregion

        #region Fields
        private SpriteRenderer renderer;

        private Transform transform;
        #endregion

        public BasicMissileSpellHandler(string name, PowerSource source, float percentageOfSource, int baseDamage)
            : base(name, source, percentageOfSource, baseDamage)
        {
        }

        protected override void DealDamage()
        {
            base.DealDamage();

            var controller = Target.FirstComponentOfType<ICharacterController>();

            controller.Statuses.Health -= LastResults.Damage;
            
            if (UserIsPlayer) GameInfoLog.Instance.LogDealDamage(LastResults.Damage, LastResults.Critical, Spell.Name, controller.Name);

            renderer.Destroy();
        }
        protected override void SendMissile()
        {
            renderer                = User.AddComponent<SpriteRenderer>();
            renderer.Sprite.Texture = Engine.Instance.Content.Load<Texture2D>(Spell.Name.ToLower());
            renderer.Sprite.Source  = new Rectangle(0, 0, 32, 32);
            renderer.Flags          = RenderFlags.AutomaticDepth;
            renderer.Sprite.Layer   = Layers.Middle;

            transform               = Target.FirstComponentOfType<Transform>();
            
            SpellHelper.ConsumeCurrencies(Controller.Specialization, Controller.Statuses, Spell);
        }
        protected override void Tick(GameTime gameTime)
        {
            if (Sensor == null) return;

            var dir = transform.Position - renderer.Sprite.Position;
            var rot = (float)Math.Atan2(transform.Position.Y - renderer.Sprite.Position.Y, transform.Position.X - renderer.Sprite.Position.X);

            dir.Normalize();

            Sensor.LinearVelocity = new Vector2(dir.X * MissileVelocity, dir.Y * MissileVelocity);
            
            renderer.Sprite.Position = ConvertUnits.ToDisplayUnits(Sensor.Position);
            renderer.Sprite.Rotation = rot;
        }
    }

    public abstract class AOESpellHandler
    {
    }
}
