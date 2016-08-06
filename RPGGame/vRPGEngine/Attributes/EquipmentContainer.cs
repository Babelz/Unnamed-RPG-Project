using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vRPGContent.Data.Enums;
using vRPGContent.Data.Items;

namespace vRPGEngine.Attributes
{
    public sealed class EquipmentContainer
    {
        #region Properties
        public Armor HeadPiece
        {
            get;
            set;
        }
        public Armor Necklace
        {
            get;
            set;
        }
        public Armor Shoulders
        {
            get;
            set;
        }
        public Armor Chest
        {
            get;
            set;
        }
        public Armor Waist
        {
            get;
            set;
        }
        public Armor Cape
        {
            get;
            set;
        }
        public Armor Wrists
        {
            get;
            set;
        }
        public Armor Hands
        {
            get;
            set;
        }
        public Armor Legs
        {
            get;
            set;
        }
        public Armor Feet
        {
            get;
            set;
        }
        public Armor Ring1
        {
            get;
            set;
        }
        public Armor Ring2
        {
            get;
            set;
        }
        public Armor Ring3
        {
            get;
            set;
        }
        public Armor Ring4
        {
            get;
            set;
        }
        public Armor Earring1
        {
            get;
            set;
        }
        public Armor Earring2
        {
            get;
            set;
        }

        public Weapon MainHand
        {
            get;
            set;
        }
        public Weapon OffHand
        {
            get;
            set;
        }

        public bool TwoHanded
        {
            get
            {
                return MainHand != null && OffHand == null &&
                       (int)(MainHand.WeaponType & WeaponType.Weapon2H) == 1;
            }
        }

        public IEnumerable<Armor> Armor
        {
            get
            {
                return new[]
                {
                    HeadPiece,
                    Necklace,
                    Shoulders,
                    Cape,
                    Chest,
                    Wrists,
                    Hands,
                    Waist,
                    Legs,
                    Feet,
                    Ring1,
                    Ring2,
                    Ring3,
                    Ring4,
                    Earring1,
                    Earring2
                };
            }
        }
        public IEnumerable<Weapon> Weapons
        {
            get
            {
                return new[]
                {
                    MainHand,
                    OffHand
                };
            }
        }
        #endregion
    }
}
