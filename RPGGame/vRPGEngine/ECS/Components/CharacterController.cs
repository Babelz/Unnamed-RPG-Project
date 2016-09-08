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
        string Name
        {
            get;
        }
        BuffContainer Buffs
        {
            get;
        }
        IEnumerable<SpellHandler> Spells
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
        #endregion

        void EnterCombat();
        void LeaveCombat();
    }
    
    public sealed class PlayerCharacterController : Component<PlayerCharacterController>, ICharacterController
    {
        #region Fields
        private readonly List<SpellHandler> spells;

        private SpellHandler casting;
        #endregion

        #region Properties
        public string Name
        {
            get
            {
                return "Player";
            }
        }

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
        public IEnumerable<SpellHandler> Spells
        {
            get
            {
                return spells;
            }
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

        public bool InCombat
        {
            get;
            private set;
        }
        #endregion

        public PlayerCharacterController()
            : base()
        {
            spells   = new List<SpellHandler>();
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

                if (handler != null) spells.Add(handler);
            }
        }

        public void Update(GameTime gameTime)
        {
            foreach (var spell in Spells) spell.Update(gameTime);

            MeleeDamageController?.Update(gameTime);
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

        #region Fields
        private readonly List<SpellHandler> spells;
        #endregion

        #region Events
        public event NPCControllerEventHandler OnDeath;
        public event NPCControllerEventHandler OnEnteringCombat;
        public event NPCControllerEventHandler OnLeavingCombat;
        public event NPCControllerEventHandler OnDecayed;
        #endregion

        #region Properties
        public string Name
        {
            get
            {
                return Handler.Data.Name;
            }
        }

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
        public IEnumerable<SpellHandler> Spells
        {
            get
            {
                return spells;
            }
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
        #endregion

        public NPCController()
            : base()
        {
            Statuses = new Statuses();
            Buffs    = new BuffContainer();
            spells   = new List<SpellHandler>();
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

                    if (Handler != null) this.spells.Add(spellHandler);
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
            if (!Statuses.Alive)
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
                    if (Handler.CombatUpdate(gameTime, spells)) CombatElapsed = 0;
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
