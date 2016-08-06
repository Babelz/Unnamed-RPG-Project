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
using vRPGContent.Data.Spells;
using vRPGEngine.ECS;

namespace vRPGEngine.Handlers.Spells
{
    public abstract class SpellHandler
    {
        #region Fields
        public string Name
        {
            get;
            private set;
        }
        public Spell Spell
        {
            get;
            private set;
        }

        public int CooldownElapsed
        {
            get;
            protected set;
        }
        public bool Working
        {
            get;
            protected set;
        }
        public bool InCooldown
        {
            get;
            protected set;
        }
        #endregion

        protected SpellHandler(string name, Spell spell)
        {
            Debug.Assert(!string.IsNullOrEmpty(name));
            Debug.Assert(spell != null);

            Name        = name;
            Spell       = spell;
        }
        
        public abstract void Use(Entity owner);

        public virtual void Update(GameTime gameTime)
        {
        }
        public virtual void Draw(SpriteBatch spriteBatch)
        {
        }
    }

    public abstract class MissileSpellHandler : SpellHandler
    {
        #region Constants
        public const int MissileDecayTime = 3000;
        #endregion

        #region Fields
        private int elapsed;
        #endregion

        #region Properties
        public float Width
        {
            get;
            protected set;
        }
        public float Height
        {
            get;
            protected set;
        }

        public Vector2 Direction
        {
            get;
            set;
        }
        public Vector2 Velocity
        {
            get;
            set;
        }
        public Entity Target
        {
            get;
            set;
        }

        public Entity Owner
        {
            get;
            private set;
        }
        public Body Sensor
        {
            get;
            private set;
        }
        #endregion

        protected MissileSpellHandler(string name, Spell spell, float width, float height)
            : base(name, spell)
        {
            Width   = width;
            Height  = height;
        }

        #region Event handlers
        private bool Collider_OnCollision(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            if (ReferenceEquals(fixtureB.Body.UserData, Target))
            {
                OnHit();

                DisposeSensor();

                return true;
            }

            return false;
        }
        #endregion
        
        private void DisposeSensor()
        {
            if (Sensor == null) return; 

            Sensor.OnCollision -= Collider_OnCollision;

            RPGWorld.Instance.DestroyBody(Sensor);

            Sensor = null;
        }
        
        protected abstract void OnHit();

        public override void Use(Entity owner)
        {
            Debug.Assert(owner != null);
            Debug.Assert(Target != null);

            Owner = owner;

            Sensor                  = RPGWorld.Instance.CreateSensor(Owner, Width, Height);
            Sensor.LinearVelocity   = Velocity;
            Sensor.OnCollision      += Collider_OnCollision;

            elapsed         = 0;
            CooldownElapsed = 0;
            Working         = true;

            if (Spell.Cooldown != 0) InCooldown = true;
        }

        public override void Update(GameTime gameTime)
        {
            if (InCooldown)
            {
                CooldownElapsed += gameTime.ElapsedGameTime.Milliseconds;

                if (CooldownElapsed >= Spell.Cooldown)
                {
                    InCooldown      = false;
                    CooldownElapsed = 0;
                }
            }

            if (Working)
            {
                elapsed += gameTime.ElapsedGameTime.Milliseconds;

                if (elapsed > MissileDecayTime)
                {
                    DisposeSensor();

                    Working = false;
                }
            }
        }
    }

    public abstract class AOESpellHandler : SpellHandler
    {
        #region Properties
        public float Radius
        {
            get;
            private set;
        }
        public Entity Owner
        {
            get;
            private set;
        }

        public int Elapsed
        {
            get;
            protected set;
        }

        public Vector2 Position
        {
            get;
            set;
        }
        #endregion

        public AOESpellHandler(string name, Spell spell, float radius)
            : base(name, spell)
        {
        }

        protected abstract void Tick(IEnumerable<Body> colliders);

        public override void Use(Entity owner)
        {
            Debug.Assert(owner != null);

            Owner           = owner;
            Working         = true;
            CooldownElapsed = 0;
            Elapsed         = 0;

            if (Spell.Cooldown != 0) InCooldown = true;
        }
        
        public override void Update(GameTime gameTime)
        {
            if (InCooldown)
            {
                CooldownElapsed += gameTime.ElapsedGameTime.Milliseconds;

                if (CooldownElapsed >= Spell.Cooldown)
                {
                    InCooldown = false;
                    CooldownElapsed = 0;
                }
            }

            if (Working)
            {
                Elapsed += gameTime.ElapsedGameTime.Milliseconds;

                var simPosition  = ConvertUnits.ToSimUnits(Position);
                var simRadius    = ConvertUnits.ToSimUnits(Radius);

                var colliders    = RPGWorld.Instance.QueryArea(simPosition, simRadius);

                if (colliders.Count() != 0) Tick(colliders);
            }
        }
    }
}