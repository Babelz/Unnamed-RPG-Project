using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vRPGContent.Data.Items.Enums
{
    [Serializable()]
    public enum EquipmentSlot : int
    {
        Head = 0,
        Neck,
        Shoulders,
        Chest,
        Cape,
        Wait,
        Hands,
        Wrists,
        Legs,
        Feet,
        Ring1,
        Ring2,
        Ring3,
        Ring4,
        Earring1,
        Earring2,
        MainHand,
        OffHand,
        BothHands = MainHand & OffHand,
        AnyHand = MainHand | OffHand,
    }
}
