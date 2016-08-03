using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vRPGEngine.Data
{
    [Serializable()]
    public sealed class Armor : Equipment
    {
        #region Properties
        public EquipmentSlot Slot
        {
            get;
            set;
        }
        #endregion

        public Armor()
            : base()
        {
            Type = ItemType.Armor;
        }
    }
}
