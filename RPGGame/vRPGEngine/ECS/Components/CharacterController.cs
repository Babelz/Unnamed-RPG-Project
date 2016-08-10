using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vRPGContent.Data.Attributes;
using vRPGContent.Data.Spells;
using vRPGEngine.Attributes;
using vRPGEngine.Combat;
using vRPGEngine.Databases;
using vRPGEngine.Handlers.NPC;
using vRPGEngine.Handlers.Spells;
using vRPGEngine.Specializations;

namespace vRPGEngine.ECS.Components
{
    public abstract class CharacterController : IComponent
    {
        #region Properties
        public int Location
        {
            get;
            set;
        }
        public Entity Owner
        {
            get;
            set;
        }

        public BuffContainer Buffs
        {
            get;
            private set;
        }
        public List<SpellHandler> Spells
        {
            get;
            private set;
        }

        public virtual AttributesData Attributes
        {
            get;
            protected set;
        }
        public Specialization Specialization
        {
            get;
            protected set;
        }
        public Statuses Statuses
        {
            get;
            protected set;
        }

        public MeleeDamageController MeleeDamageController
        {
            get;
            protected set;
        }
        public ITargetFinder TargetFinder
        {
            get;
            protected set;
        }

        public bool Alive
        {
            get
            {
                return Statuses.Health != 0;
            }
        }
        public bool HasFocus
        {
            get
            {
                return Statuses.Focus != 0;
            }
        }
        public bool HasMana
        {
            get
            {
                return Statuses.Mana != 0;
            }
        }
        #endregion

        public CharacterController()
        {
            Buffs = new BuffContainer();
            Spells = new List<SpellHandler>();
            Statuses = new Statuses();
        }

        protected void CharacterControllerInitialize()
        {
            Debug.Assert(Statuses != null);
            Debug.Assert(Specialization != null);

            Statuses.Health = Specialization.TotalHealth();
            Statuses.Focus = Specialization.TotalFocus();
            Statuses.Mana = Specialization.TotalMana();

            // Load spells.
            foreach (var spell in Specialization.Spells)
            {
                var handler = SpellHandlerFactory.Instance.Create(spell, spell.HandlerName);

                if (handler != null) Spells.Add(handler);
            }

            // Add spells that everyone has.
            Spells.Add(new AutoAttack());
        }

        public abstract void Destroy();
    }
    
    public sealed class PlayerCharacterController : CharacterController
    {
        #region Fields
        private SpellHandler casting;
        #endregion

        #region Properties
        public EquipmentContainer Equipments
        {
            get;
            private set;
        }
        public override AttributesData Attributes
        {
            get;
            protected set;
        }
        #endregion

        public PlayerCharacterController()
            : base()
        {
            TargetFinder = new TargetFinder();
        }

        public void Initialize(Specialization specialization, AttributesData attributes, EquipmentContainer equipments, MeleeDamageController meleeDamageController, Statuses statuses)
        {
            Debug.Assert(specialization != null);
            Debug.Assert(attributes != null);
            Debug.Assert(equipments != null);
            Debug.Assert(meleeDamageController != null);

            Specialization          = specialization;
            Attributes              = attributes;
            Equipments              = equipments;
            MeleeDamageController   = meleeDamageController;
            Statuses                = statuses;

            CharacterControllerInitialize();

            // Add spells that everyone has.
            Spells.Add(new AutoAttack());
        }

        public void Update(GameTime gameTime)
        {
            foreach (var spell in Spells) spell.Update(gameTime);
        }

        public void BeginCast(int id)
        {
            if (casting != null)
            {
            }

            casting = Spells.FirstOrDefault(h => h.Spell.ID == id);
        }

        public override void Destroy()
        {
            ComponentManager<PlayerCharacterController>.Instance.Destroy(this);
        }
    }

    public sealed class NPCController : CharacterController
    {
        #region Const fields
        public const int CombatInactiveTime = 5000;
        public const int DecayTime = 25000;
        #endregion

        #region Events
        public event NPCControllerEventHandler OnDeath;
        public event NPCControllerEventHandler OnEnteringCombat;
        public event NPCControllerEventHandler OnLeavingCombat;
        public event NPCControllerEventHandler OnDecayed;
        #endregion

        #region Properties
        public NPCHandler Handler
        {
            get;
            private set;
        }

        public int CombatElapsed
        {
            get;
            private set;
        }
        public int DecayElapsed
        {
            get;
            private set;
        }
        public bool InCombat
        {
            get;
            private set;
        }

        public override AttributesData Attributes
        {
            get
            {
                return Handler.Data;
            }
            protected set
            {
                Handler.Data.CopyAttributes(value);
            }
        }
        #endregion

        public NPCController()
            : base()
        {
        }

        public void Initialize(NPCHandler handler)
        {
            Handler = handler;

            if (handler.Data.SpellList == null) return;

            var spells = SpellDatabase.Instance.Elements().Where(p => handler.Data.SpellList.Contains(p.ID));

            // Load spells.
            foreach (var spell in spells)
            {
                var spellHandler = SpellHandlerFactory.Instance.Create(spell, spell.HandlerName);

                if (handler != null) Spells.Add(spellHandler);
            }
        }

        public void EnterCombat()
        {
            if (InCombat) return;

            InCombat = true;

            Handler.EnterCombat();

            OnEnteringCombat?.Invoke(this);
        }
        public void LeaveCombat()
        {
            if (!InCombat) return;

            InCombat = false;

            Handler.LeaveCombat();

            OnLeavingCombat?.Invoke(this);
        }

        public void Update(GameTime gameTime)
        {
            // Death/decay update.
            if (!Alive)
            {
                // Death update.
                OnDeath?.Invoke(this);

                OnDeath = null;

                DecayElapsed += gameTime.ElapsedGameTime.Milliseconds;

                // Decay update.
                if (DecayElapsed > DecayTime)
                {
                    OnDecayed?.Invoke(this);

                    OnDecayed = null;
                }

                return;
            }

            // Combat update.
            if (InCombat)
            {
                if (CombatElapsed > CombatInactiveTime)
                {
                    LeaveCombat();

                    InCombat = false;

                    CombatElapsed = 0;
                }
                else
                {
                    if (Handler.CombatUpdate(gameTime, Spells)) CombatElapsed = 0;
                    else CombatElapsed += gameTime.ElapsedGameTime.Milliseconds;
                }

                return;
            }

            // Idle update.
            Handler.IdleUpdate(gameTime);
        }

        public override void Destroy()
        { 
            ComponentManager<NPCController>.Instance.Destroy(this);
        }
  
        public delegate void NPCControllerEventHandler(NPCController controller);
    }
}
