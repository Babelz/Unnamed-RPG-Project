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
using vRPGEngine.Attributes.Specializations;
using vRPGEngine.Combat;
using vRPGEngine.Databases;
using vRPGEngine.Handlers.NPC;
using vRPGEngine.Handlers.Spells;
using vRPGEngine.Specializations;

namespace vRPGEngine.ECS.Components
{
    public interface ICharacterController : IComponent
    {
        #region Properties
        BuffContainer Buffs
        {
            get;
        }
        List<SpellHandler> Spells
        {
            get;
        }

        AttributesData Attributes
        {
            get;
        }
        Specialization Specialization
        {
            get;
        }
        Statuses Statuses
        {
            get;
        }

        MeleeDamageController MeleeDamageController
        {
            get;
        }
        ITargetFinder TargetFinder
        {
            get;
        }

        bool Alive
        {
            get;
        }
        bool HasFocus
        {
            get;
        }
        bool HasMana
        {
            get;
        }
        #endregion

        void EnterCombat();
        void LeaveCombat();
    }
    
    public sealed class PlayerCharacterController : Component<PlayerCharacterController>, ICharacterController
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

        public AttributesData Attributes
        {
            get;
            private set;
        }
        public Specialization Specialization
        {
            get;
            private set;
        }
        public Statuses Statuses
        {
            get;
            private set;
        }

        public MeleeDamageController MeleeDamageController
        {
            get;
            private set;
        }
        public ITargetFinder TargetFinder
        {
            get;
            private set;
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

        public bool InCombat
        {
            get;
            private set;
        }
        #endregion

        public PlayerCharacterController()
            : base()
        {
            InCombat = false;
        }
        
        #region Event handlers
        private void Instance_HostilesEmpty()
        {
            LeaveCombat();
        }
        private void Instance_HostileRegistered()
        {
            EnterCombat();
        }
        #endregion

        public void Initialize(Specialization specialization, AttributesData attributes, EquipmentContainer equipments, MeleeDamageController meleeDamageController, Statuses statuses)
        {
            Debug.Assert(specialization != null);
            Debug.Assert(attributes != null);
            Debug.Assert(equipments != null);
            Debug.Assert(meleeDamageController != null);

            Buffs           = new BuffContainer();
            TargetFinder    = new TargetFinder();
            Spells          = new List<SpellHandler>();

            CombatManager.Instance.HostileRegistered += Instance_HostileRegistered;
            CombatManager.Instance.HostilesEmpty     += Instance_HostilesEmpty;
        
            Specialization          = specialization;
            Attributes              = attributes;
            Equipments              = equipments;
            MeleeDamageController   = meleeDamageController;
            Statuses                = statuses;

            Statuses.Initialize(specialization);

            foreach (var spell in specialization.Spells)
            {
                var handler = SpellHandlerFactory.Instance.Create(spell.HandlerName);

                if (handler != null) Spells.Add(handler);
            }

            // Add spells that everyone has.
            Spells.Add(new AutoAttack());
        }

        public void Update(GameTime gameTime)
        {
            foreach (var spell in Spells) spell.Update(gameTime);

            MeleeDamageController?.Update(gameTime);
        }

        public void BeginCast(int id)
        {
            if (casting != null)
            {
            }

            casting = Spells.FirstOrDefault(h => h.Spell.ID == id);
        }
        
        public void EnterCombat()
        {
            if (InCombat) return;
            
            InCombat = true;

            MeleeDamageController?.EnterCombat();
        }
        public void LeaveCombat()
        {
            if (!InCombat) return;
            
            InCombat = false;

            MeleeDamageController?.LeaveCombat();
            TargetFinder.ClearTarget();
        }
    }

    public sealed class NPCController : Component<NPCController>, ICharacterController
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
            set;
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

        public AttributesData Attributes
        {
            get
            {
                return Handler.Data;
            }
            private set
            {
                Handler.Data.CopyAttributes(value);
            }
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
        public Specialization Specialization
        {
            get;
            private set;
        }
        public Statuses Statuses
        {
            get;
            private set;
        }

        public MeleeDamageController MeleeDamageController
        {
            get;
            private set;
        }
        public ITargetFinder TargetFinder
        {
            get;
            private set;
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

        public NPCController()
            : base()
        {
            Statuses = new Statuses();
            Buffs    = new BuffContainer();
            Spells   = new List<SpellHandler>();
        }

        public new void Initialize()
        {
            Debug.Assert(Handler != null);
            
            if (string.IsNullOrEmpty(Handler.Data.SpecializationName))
            {
                var specialization = new DefaultNPCSpecialization(Attributes, Statuses);
                Statuses.Initialize(specialization);
            }
            else
            {
                throw new NotImplementedException("can't load specializations");
            }

            if (Handler.Data.SpellList != null)
            {
                var spells = SpellDatabase.Instance.Elements().Where(p => Handler.Data.SpellList.Contains(p.ID));

                // Load spells.
                foreach (var spell in spells)
                {
                    var spellHandler = SpellHandlerFactory.Instance.Create(spell.HandlerName);

                    if (Handler != null) Spells.Add(spellHandler);
                }
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

        public delegate void NPCControllerEventHandler(NPCController controller);
    }
}
