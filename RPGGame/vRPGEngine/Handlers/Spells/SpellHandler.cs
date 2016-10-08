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
using vRPGEngine.Combat;
using vRPGEngine.Core;
using vRPGEngine.Databases;
using vRPGEngine.ECS;
using vRPGEngine.ECS.Components;
using vRPGEngine.Graphics;
using vRPGEngine.Interfaces;

namespace vRPGEngine.Handlers.Spells
{
    public abstract class SpellHandler : IGenericCloneable<SpellHandler>
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

        public bool BeingUsed
        {
            get;
            protected set;
        }
        public bool OnCooldown
        {
            get;
            protected set;
        }
        #endregion

        protected SpellHandler(string name)
        {
            Debug.Assert(!string.IsNullOrEmpty(name));
            
            Spell       = SpellDatabase.Instance.Elements().FirstOrDefault(e => e.Name.ToLower() == name);

            if (Spell == null) Logger.Instance.LogError(string.Format("could not load spell named \"{0}\"", name));
        }
        
        public abstract void Use(Entity user);

        public virtual void Update(GameTime gameTime)
        {
        }
        public virtual void Present(SpriteBatch spriteBatch, GameTime gameTime)
        {
        }

        public abstract SpellHandler Clone();
    }

    public abstract class MissileSpellHandler : SpellHandler
    {
        #region Constants
        public const float MissileVelocity  = 0.5f;
        public const int MissileDecayTime   = 3000;
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

        protected MissileSpellHandler(string name, float width, float height)
            : base(name)
        {
            Width   = width;
            Height  = height;
        }
        protected MissileSpellHandler(string name)
            : this(name, 32.0f, 32.0f)
        {
        }
        
        // TODO: add spell decaying...

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
        protected abstract void OnUse();

        protected void Send()
        {
            var position        = Owner.FirstComponentOfType<Transform>().Position;

            Sensor              = RPGWorld.Instance.CreateBoxSensor(Owner, position, Width, Height);
            
            Sensor.OnCollision  += Collider_OnCollision;
        }

        public override void Use(Entity owner)
        {
            if (OnCooldown) return;

            Owner = owner;
            Debug.Assert(owner != null);

            Target = owner.FirstComponentOfType<ICharacterController>().TargetFinder.Target;
            Debug.Assert(Target != null);
            
            CooldownElapsed = 0;
            BeingUsed       = true;
            
            if (Spell.Cooldown != 0) OnCooldown = true;

            OnUse();
        }

        public override void Update(GameTime gameTime)
        {
            if (OnCooldown)
            {
                CooldownElapsed += gameTime.ElapsedGameTime.Milliseconds;

                if (CooldownElapsed >= Spell.Cooldown)
                {
                    OnCooldown      = false;
                    CooldownElapsed = 0;
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
            Inactive,

            /// <summary>
            /// The spell is still being used, the handler should not be disposed.
            /// </summary>
            Active
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

        public MeleeSpellHandler(string name)
            : base(name)
        {
        }

        protected abstract MeleeSpellState OnUse(GameTime gameTime);

        protected bool CanUse()
        {
            if (UserController.TargetFinder.Target == null)                                          return false;
            if (!SpellHelper.InRange(UserController, User, Spell))                                   return false;
            if (!SpellHelper.CanUse(UserController.Specialization, UserController.Statuses, Spell))  return false;

            return true;
        }

        private void Enable(Entity user, ICharacterController userController)
        {
            User            = user;
            UserController  = userController;

            // Try toggle.
            if (UserController.TargetFinder.TargetController == null)
            {
                Disable();

                return;
            }

            if (!SpellHelper.InRange(UserController, user, Spell))
            {
                if (user.Tagged("player")) GameInfoLog.Instance.LogRaw("target is too far away!", InfoLogEntryType.Warning);

                Disable();

                return;
            }
            if (ReferenceEquals(UserController.TargetFinder.Target, User))
            {
                if (user.Tagged("player")) GameInfoLog.Instance.LogRaw("can't attack yourself!", InfoLogEntryType.Warning);

                Disable();

                return;
            }

            BeingUsed = true;

            UserController.EnterCombat();
            userController.TargetFinder.TargetController.EnterCombat();
               
            return;
        }

        private void Disable()
        {
            BeingUsed = false;
        }

        public override void Use(Entity user)
        {
            var controller = user.FirstComponentOfType<ICharacterController>();
            
            if (Spell.GCD && GlobalCooldownManager.Instance.IsInCooldown(controller)) return;
            if (OnCooldown)                                                           return;

            if (BeingUsed)
            {
                Disable();

                return;
            }
            else
            {
                Enable(user, controller);

                if (BeingUsed)
                    if (Spell.GCD)
                        GlobalCooldownManager.Instance.Trigger(user.FirstComponentOfType<ICharacterController>());
            }
        }

        public override void Update(GameTime gameTime)
        {
            if (!BeingUsed)                                                               return;
            if (Spell.GCD && GlobalCooldownManager.Instance.IsInCooldown(UserController)) return;

            BeingUsed = OnUse(gameTime) == MeleeSpellState.Active;

            if (!BeingUsed) Disable();
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

        public AOESpellHandler(string name, float radius)
            : base(name)
        {
        }

        protected abstract void Tick(IEnumerable<Body> colliders);

        public override void Use(Entity user)
        {
            Debug.Assert(user != null);

            User                = user;
            BeingUsed           = true;
            CooldownElapsed     = 0;
            Elapsed             = 0;

            if (Spell.Cooldown != 0) OnCooldown = true;
        }
        
        public override void Update(GameTime gameTime)
        {
            if (OnCooldown)
            {
                CooldownElapsed += gameTime.ElapsedGameTime.Milliseconds;

                if (CooldownElapsed >= Spell.Cooldown)
                {
                    OnCooldown = false;
                    CooldownElapsed = 0;
                }
            }

            if (BeingUsed)
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

        protected SelfBuffSpellHandler(string name, int decayTime, IRenderable renderable)
                : base(name)
        {
            DecayTime  = decayTime;
            Renderable = renderable;
        }

        protected virtual bool RefreshIfCan(Buff buff)
        {
            return true;
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
            
            if (Spell.GCD && GlobalCooldownManager.Instance.IsInCooldown(UserController)) return;

            BeingUsed       = true;
            Elapsed         = 0;
            CooldownElapsed = 0;

            if (Spell.Cooldown != 0) OnCooldown = true;

            buff = UserController.Buffs.Buffs.FirstOrDefault(b => b.FromSpell.ID == Spell.ID);

            if (buff != null)
            {
                if (RefreshIfCan(buff))
                    GlobalCooldownManager.Instance.Trigger(UserController);
            }
            else
            {
                buff = UseIfCan();

                if (buff == null)   BeingUsed = false;
                else                GlobalCooldownManager.Instance.Trigger(UserController);
            }
        }

        public override void Update(GameTime gameTime)
        {
            if (OnCooldown)
            {
                CooldownElapsed += gameTime.ElapsedGameTime.Milliseconds;

                if (CooldownElapsed >= Spell.Cooldown)
                {
                    OnCooldown      = false;
                    CooldownElapsed = 0;
                }
            }

            if (!BeingUsed) return;

            Elapsed += gameTime.ElapsedGameTime.Milliseconds;

            if (Elapsed > DecayTime)
            {
                Remove();

                BeingUsed = false;
            }
        }

        public override void Present(SpriteBatch spriteBatch, GameTime gameTime)
        {
            if (renderable == null) return;

            renderable.Present(spriteBatch, gameTime);
        }
    }
}