using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vRPGContent.Data.Items.Enums;

namespace vRPGContent.Data.Items
{
    [Serializable()]
    public sealed class Armor : Equipment
    {
        #region Properties
        public int ArmorValue
        {
            get;
            set;
        }
        public EquipmentSlot Slot
        {
            get;
            set;
        }
        public ArmorType ArmorType
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
