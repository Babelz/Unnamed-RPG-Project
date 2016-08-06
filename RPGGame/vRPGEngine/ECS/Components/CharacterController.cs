using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vRPGEngine.Attributes;
using vRPGEngine.Handlers.Spells;
using vRPGEngine.Specializations;

namespace vRPGEngine.ECS.Components
{
    public sealed class CharacterController : Component<CharacterController>
    {
        #region Properties
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

        public Specialization Specialization
        {
            get;
            set;
        }
        public AttributesData Attributes
        {
            get;
            set;
        }
        public List<SpellHandler> Spells
        {
            get;
            set;
        }
        #endregion

        public CharacterController()
            : base()
        {
            Buffs       = new BuffContainer();
            Equipments  = new EquipmentContainer();
        }
    }
}
