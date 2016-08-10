using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vRPGContent.Data.Spells;
using vRPGEngine.Attributes;
using vRPGEngine.Combat;
using vRPGEngine.Databases;
using vRPGEngine.Handlers.NPC;
using vRPGEngine.Handlers.Spells;
using vRPGEngine.Specializations;

namespace vRPGEngine.ECS.Components
{
    public sealed class CharacterController : Component<CharacterController>
    {
        #region Fields
        private SpellHandler casting;
        #endregion

        #region Properties
        public AttributesData Attributes
        {
            get;
            private set;
        }
        public BuffContainer Buffs
        {
            get;
            private set;
        }
        public EquipmentContainer Equipments
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
        public MeleeDamageController MeleeDamageController
        {
            get;
            private set;
        }
        public TargetFinder TargetFinder
        {
            get;
            private set;
        }
        public Statuses Statuses
        {
            get;
            private set;
        }

        // TODO: talents data
        //          - trees
        //          - spells
        //          - passives
        #endregion

        public CharacterController()
            : base()
        {
            Buffs        = new BuffContainer();
            Spells       = new List<SpellHandler>();
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

            // Compute statuses.
            statuses.Health = specialization.TotalHealth();
            statuses.Focus  = specialization.TotalFocus();
            statuses.Mana   = specialization.TotalMana();

            // Load spells.
            foreach (var spell in specialization.Spells)
            {
                var handler = SpellHandlerFactory.Instance.Create(spell, spell.HandlerName);

                if (handler != null) Spells.Add(handler);
            }
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
    }

    public sealed class NPCController : Component<NPCController>
    {
        #region Const fields
        public const int CombatInactiveTime = 500;
        public const int DecayTime          = 2500;
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
        public List<SpellHandler> Spells
        {
            get;
            private set;
        }
        public BuffContainer Buffs
        {
            get;
            private set;
        }

        public bool Alive
        {
            get
            {
                return Handler.Data.Health > 0;
            }
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
        #endregion
        
        public NPCController()
            : base()
        {
            Spells = new List<SpellHandler>();
            Buffs  = new BuffContainer();
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

            Handler.EnterCombat();

            OnEnteringCombat?.Invoke(this);
        }
        public void LeaveCombat()
        {
            if (!InCombat) return;

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
                    Handler.Die(gameTime);

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
                    else                                        CombatElapsed += gameTime.ElapsedGameTime.Milliseconds;
                }

                return;
            }

            // Idle update.
            Handler.IdleUpdate(gameTime);
        }

        public delegate void NPCControllerEventHandler(NPCController controller);
    }
}
