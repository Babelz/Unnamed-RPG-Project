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
using vRPGEngine.Attributes.Spells;
using vRPGEngine.ECS;
using vRPGEngine.ECS.Components;
using vRPGEngine.Graphics;

namespace vRPGEngine.Handlers.Spells
{
    public abstract class SpellHandler : ICloneable
    {
        #region Properties
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
        public bool InUse
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

        protected SpellHandler(Spell spell)
        {
            Debug.Assert(spell != null);
            
            Spell       = spell;
        }
        
        public abstract void Use(Entity user);

        public virtual void Update(GameTime gameTime)
        {
        }
        public virtual void Present(SpriteBatch spriteBatch, GameTime gameTime)
        {
        }

        public abstract object Clone();
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

        protected MissileSpellHandler(Spell spell, float width, float height)
            : base(spell)
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
            InUse         = true;

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

            if (InUse)
            {
                elapsed += gameTime.ElapsedGameTime.Milliseconds;

                if (elapsed > MissileDecayTime)
                {
                    DisposeSensor();

                    InUse = false;
                }
            }
        }
    }

    public abstract class MeleeSpellHandler : SpellHandler
    {
        #region Enums
        public enum MeleeSpellState
        {
            /// <summary>
            /// The spell has been used, the handler should be disposed.
            /// </summary>
            Used,

            /// <summary>
            /// The spell is still being used, the handler should not be disposed.
            /// </summary>
            Using
        }
        #endregion

        #region Properties
        protected Entity User
        {
            get;
            private set;
        }
        protected ICharacterController UserController
        {
            get;
            private set;
        }
        #endregion

        public MeleeSpellHandler(Spell spell)
            : base(spell)
        {
        }

        protected abstract MeleeSpellState Tick(GameTime gameTime);

        private void Toggle(Entity user)
        {
            User = user;

            UserController = user.FirstComponentOfType<ICharacterController>();

            if (InUse)
            {
                UserController.MeleeDamageController.LeaveCombat();

                InUse = false;

                return;
            }

            if (UserController == null)                               return;
            if (UserController.TargetFinder.TargetController == null) return;

            if (!MeleeHelper.InRange(UserController, user, Spell))
            {
                if (user.Tags == "player") GameInfoLog.Instance.LogRaw("target is too far away!", InfoLogEntryType.Warning);

                return;
            }
            if (ReferenceEquals(UserController.TargetFinder.Target, User))
            {
                if (user.Tags == "player") GameInfoLog.Instance.LogRaw("can't attack yourself!", InfoLogEntryType.Warning);

                return;
            }

            InUse = true;

            UserController.EnterCombat();
            UserController.TargetFinder.TargetController.EnterCombat();

            return;
        }

        public override void Use(Entity user)
        {
            Toggle(user);
        }

        public override void Update(GameTime gameTime)
        {
            if (!InUse) return;

            InUse = Tick(gameTime) == MeleeSpellState.Using;
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
        public Entity User
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

        public AOESpellHandler(Spell spell, float radius)
            : base(spell)
        {
        }

        protected abstract void Tick(IEnumerable<Body> colliders);

        public override void Use(Entity user)
        {
            Debug.Assert(user != null);

            User           = user;
            InUse         = true;
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

            if (InUse)
            {
                Elapsed += gameTime.ElapsedGameTime.Milliseconds;

                var simPosition  = ConvertUnits.ToSimUnits(Position);
                var simRadius    = ConvertUnits.ToSimUnits(Radius);

                var colliders    = RPGWorld.Instance.QueryArea(simPosition, simRadius);

                if (colliders.Count() != 0) Tick(colliders);
            }
        }
    }

    public abstract class SelfBuffSpellHandler : SpellHandler
    {
        #region Fields
        private readonly IRenderable renderable;
        
        private Buff buff;
        #endregion

        #region Properties
        public int DecayTime
        {
            get;
            private set;
        }

        public int Elapsed
        {
            get;
            protected set;
        }

        public Entity User
        {
            get;
            protected set;
        }
        public ICharacterController UserController
        {
            get;
            protected set;
        }
        public IRenderable Renderable
        {
            get;
            protected set;
        }
        #endregion

        protected SelfBuffSpellHandler(Spell spell, int decayTime, IRenderable renderable)
                : base(spell)
        {
            DecayTime  = decayTime;
            Renderable = renderable;
        }

        protected virtual void RefreshIfCan(Buff buff)
        {
        }
        protected virtual Buff UseIfCan()
        {
            return null;
        }

        public void Remove()
        {
            UserController.Buffs.Remove(buff);

            buff.Remove(User);
        }

        public override void Use(Entity user)
        {
            User            = user;
            UserController  = user.FirstComponentOfType<ICharacterController>();
            InUse         = true;
            Elapsed         = 0;
            CooldownElapsed = 0;

            if (Spell.Cooldown != 0) InCooldown = true;

            buff = UserController.Buffs.Buffs.FirstOrDefault(b => b.FromSpell.ID == Spell.ID);

            if (buff != null)
            {
                RefreshIfCan(buff);
            }
            else
            {
                buff = UseIfCan();

                if (buff == null) InUse = false;
            }
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

            if (!InUse) return;

            Elapsed += gameTime.ElapsedGameTime.Milliseconds;

            if (Elapsed > DecayTime)
            {
                Remove();

                InUse = false;
            }
        }

        public override void Present(SpriteBatch spriteBatch, GameTime gameTime)
        {
            if (renderable == null) return;

            renderable.Present(spriteBatch, gameTime);
        }
    }
}