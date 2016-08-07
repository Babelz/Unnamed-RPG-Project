using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vRPGContent.Data.Spells;
using vRPGEngine.Attributes;
using vRPGEngine.Handlers.Spells;
using vRPGEngine.Specializations;

namespace vRPGEngine.ECS.Components
{
    public interface ICharacterController
    {
        #region Properties
        BuffContainer Buffs
        {
            get;
        }
        AttributesData Attributes
        {
            get;
        }
        List<SpellHandler> Spells
        {
            get;
        }
        #endregion
    }

    public sealed class PlayerCharacterController : Component<NonPlayerCharacterController>, ICharacterController
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

        // TODO: talents data
        //          - trees
        //          - spells
        //          - passives
        #endregion

        public PlayerCharacterController()
            : base()
        {
            Attributes  = new AttributesData();
            Buffs       = new BuffContainer();
            Equipments  = new EquipmentContainer();
            Spells      = new List<SpellHandler>();
        }

        public void SetSpecialization(Specialization specialization)
        {
            Debug.Assert(specialization != null);

            Specialization = specialization;

            // Load spells.
            foreach (var spell in specialization.Spells)
            {
                var handler = SpellHandlerFactory.Create(spell);

                if (handler != null) Spells.Add(handler);
            }
        }

        public void BeginCast(int id)
        {
            if (casting != null)
            {
            }

            casting = Spells.FirstOrDefault(h => h.Spell.ID == id);
        }
    }

    public sealed class NonPlayerCharacterController : Component<NonPlayerCharacterController>, ICharacterController
    {
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
        public List<SpellHandler> Spells
        {
            get;
            private set;
        }

        // TODO: NPC data:
        //          - name
        //          - attribs
        //          - spells
        //          - animations 
        //          - sounds
        //          - sprites etc
        //          - faction
        //          - type
        #endregion
    }
}
