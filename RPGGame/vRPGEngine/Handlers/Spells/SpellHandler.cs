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
using vRPGEngine.Attributes;
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
    public enum SpellState : byte
    {
        Inactive = 0,
        Active,
        Used
    }

    public abstract class SpellHandler : IGenericCloneable<SpellHandler>
    {
        #region Fields
        private SpellState state;
        #endregion

        #region Properties
        protected Entity User
        {
            get;
            private set;
        }
        protected ICharacterController Controller
        {
            get;
            private set;
        }

        protected bool UserIsPlayer
        {
            get;
            private set;
        }

        protected Entity Target
        {
            get;
            private set;
        }

        public Spell Spell
        {
            get;
            private set;
        }

        public bool InCooldown
        {
            get
            {
                return CooldownElapsed != 0;
            }
        }
        public int CooldownElapsed
        {
            get;
            private set;
        }
        #endregion

        public SpellHandler(Spell spell)
        {
            Debug.Assert(spell != null);

            Spell = spell;
        }

        private void InitializeUserdata(Entity user)
        {
            User            = user;
            
            Controller      = user.FirstComponentOfType<ICharacterController>();

            UserIsPlayer    = user.Tagged("player");

            Target          = Controller.TargetFinder.Target;
        }
        private void UnitializeUserdata()
        {
            User            = null;

            Controller      = null;

            UserIsPlayer    = false;

            Target          = null;
        }

        private void ResetStateInformation()
        {
            CooldownElapsed   = 0;

            state           = SpellState.Inactive;
        }

        private bool NotInCooldown()
        {
            return !GlobalCooldownManager.Instance.IsInCooldown(Controller) && !InCooldown;
        }
        private bool CanAfford()
        {
            return SpellHelper.CanAfford(Controller.Specialization, Controller.Statuses, Spell);
        }
        private bool InRange()
        {
            return SpellHelper.InRange(Controller, User, Spell);
        }
        private bool HasValidTarget()
        {
            if (UserIsPlayer && Target.Tagged("player")) return false;

            return Target != null;
        }
        private bool TargetIsDead()
        {
            return Target.FirstComponentOfType<ICharacterController>().Statuses.Alive;
        }

        private bool IsBeingUsed()
        {
            return User != null;
        }

        protected virtual bool CanUse()
        {
            if (!NotInCooldown())
            {
                if (UserIsPlayer) GameInfoLog.Instance.LogWarning("can't use spell while on cooldown!");

                return false;
            }

            if (!CanAfford())
            {
                if (UserIsPlayer) GameInfoLog.Instance.LogWarning("can't afford that spell!");

                return false;
            }

            if (!HasValidTarget())
            {
                if (UserIsPlayer) GameInfoLog.Instance.LogWarning("no target to attack!");

                return false;
            }

            if (!InRange())
            {
                if (UserIsPlayer) GameInfoLog.Instance.LogWarning("out of range!");

                return false;
            }

            if (!TargetIsDead())
            {
                if (UserIsPlayer) GameInfoLog.Instance.LogWarning("target is dead");

                return false;
            }
            
            return true;
        }
        
        /// <summary>
        /// Called in every update call if the spell is in active state.
        /// Not all spells requires a tick function but some do.
        /// </summary>
        protected virtual void Tick(GameTime gameTime)
        {
        }
        /// <summary>
        /// Called when the use method gets called.
        /// Used to initialize different types of spells and
        /// do spell specific initialization logic.
        /// </summary>
        protected virtual void Use()
        {
        }
        /// <summary>
        /// Should be called when the spell gets sent/used and 
        /// a cooldown should be triggered.
        /// This method is the last called function in the spells
        /// function call tree. It should deal damage to the target.
        /// </summary>
        protected virtual void DealDamage()
        {
            state = SpellState.Used;
        }

        public virtual void Interrupt()
        {
            UnitializeUserdata();
            ResetStateInformation();
        }

        public void Use(Entity user)
        {
            Debug.Assert(user != null);

            if (state != SpellState.Inactive) return;

            UnitializeUserdata();
            ResetStateInformation();

            InitializeUserdata(user);

            // Always check that the spell can be used and
            // trigger the global cooldown if the spell 
            // should trigger it.
            if (!CanUse()) return;
            if (Spell.GCD) GlobalCooldownManager.Instance.Trigger(Controller);
            
            state = SpellState.Active;

            Use();
        }

        public void Update(GameTime gameTime)
        {
            switch (state)
            {
                case SpellState.Active:
                    Tick(gameTime);
                    break;
                case SpellState.Used:
                    CooldownElapsed += gameTime.ElapsedGameTime.Milliseconds;

                    if (CooldownElapsed >= Spell.Cooldown)
                    {
                        UnitializeUserdata();
                        ResetStateInformation();
                    }
                    break;
                case SpellState.Inactive:
                default:
                    break;
            }
        }

        public abstract SpellHandler Clone();
    }

    public abstract class BuffSpellHandler : SpellHandler
    {
        #region Properties
        public int DecayTime
        {
            get;
            private set;
        }

        public int Elapsed
        {
            get;
            private set;
        }
        #endregion

        public BuffSpellHandler(string name) 
            : base(SpellDatabase.Instance.First(e => e.Name == name))
        {
        }
    }
}