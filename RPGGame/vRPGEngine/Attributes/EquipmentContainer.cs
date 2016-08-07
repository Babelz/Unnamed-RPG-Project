using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vRPGContent.Data.Enums;
using vRPGContent.Data.Items;

namespace vRPGEngine.Attributes
{
    public enum WeaponSlot : int
    {
        MainHand = 0,
        OffHand,
        Both
    }

    public sealed class EquipmentContainer
    {
        #region Fields
        private Armor headPiece;
        private Armor necklace;
        private Armor shoulders;
        private Armor chest;
        private Armor waist;
        private Armor cape;
        private Armor wrists;
        private Armor hands;
        private Armor legs;
        private Armor feet;
        private Armor ring1;
        private Armor ring2;
        private Armor ring3;
        private Armor ring4;
        private Armor earring1;
        private Armor earring2;

        private Weapon mainHand;
        private Weapon offHand;
        #endregion

        #region Properties
        public Armor HeadPiece
        {
            get
            {
                return headPiece;
            }
            set
            {
                if (headPiece != value) EquipmentChanged?.Invoke(headPiece, value);

                headPiece = value;
            }
        }
        public Armor Necklace
        {
            get
            {
                return necklace;
            }
            set
            {
                if (necklace != value) EquipmentChanged?.Invoke(necklace, value);

                necklace = value;
            }
        }
        public Armor Shoulders
        {
            get
            {
                return shoulders;
            }
            set
            {
                if (shoulders != value) EquipmentChanged?.Invoke(shoulders, value);

                shoulders = value;
            }
        }
        public Armor Chest
        {
            get
            {
                return chest;
            }
            set
            {
                if (chest != value) EquipmentChanged?.Invoke(chest, value);

                chest = value;
            }
        }
        public Armor Waist
        {
            get
            {
                return waist;
            }
            set
            {
                if (waist != value) EquipmentChanged?.Invoke(waist, value);

                waist = value;
            }
        }
        public Armor Cape
        {
            get
            {
                return cape;
            }
            set
            {
                if (cape != value) EquipmentChanged?.Invoke(cape, value);

                cape = value;
            }
        }
        public Armor Wrists
        {
            get
            {
                return wrists;
            }
            set
            {
                if (wrists != value) EquipmentChanged?.Invoke(wrists, value);

                wrists = value;
            }
        }
        public Armor Hands
        {
            get
            {
                return hands;
            }
            set
            {
                if (hands != value) EquipmentChanged?.Invoke(hands, value);

                hands = value;
            }
        }
        public Armor Legs
        {
            get
            {
                return legs;
            }
            set
            {
                if (legs != value) EquipmentChanged?.Invoke(legs, value);

                legs = value;
            }
        }
        public Armor Feet
        {
            get
            {
                return feet;
            }
            set
            {
                if (feet != value) EquipmentChanged?.Invoke(feet, value);

                feet = value;
            }
        }
        public Armor Ring1
        {
            get
            {
                return ring1;
            }
            set
            {
                if (ring1 != value) EquipmentChanged?.Invoke(ring1, value);

                ring1 = value;
            }
        }
        public Armor Ring2
        {
            get
            {
                return ring2;
            }
            set
            {
                if (ring2 != value) EquipmentChanged?.Invoke(ring2, value);

                ring2 = value;
            }
        }
        public Armor Ring3
        {
            get
            {
                return ring3;
            }
            set
            {
                if (ring3 != value) EquipmentChanged?.Invoke(ring3, value);

                ring3 = value;
            }
        }
        public Armor Ring4
        {
            get
            {
                return ring4;
            }
            set
            {
                if (ring4 != value) EquipmentChanged?.Invoke(ring4, value);

                ring4 = value;
            }
        }
        public Armor Earring1
        {
            get
            {
                return earring1;
            }
            set
            {
                if (earring1 != value) EquipmentChanged?.Invoke(earring1, value);

                earring1 = value;
            }
        }
        public Armor Earring2
        {
            get
            {
                return earring2;
            }
            set
            {
                if (earring2 != value) EquipmentChanged?.Invoke(earring2, value);

                earring2 = value;
            }
        }
        public Weapon MainHand
        {
            get
            {
                return mainHand;
            }
            set
            {
                if (mainHand != value) WeaponChanged?.Invoke(mainHand, value, WeaponSlot.MainHand);

                mainHand = value;
            }
        }
        public Weapon OffHand
        {
            get
            {
                return offHand;
            }

            set
            {
                if (offHand != value) WeaponChanged?.Invoke(offHand, value, WeaponSlot.OffHand);

                offHand = value;
            }
        }

        public bool TwoHanded
        {
            get
            {
                return MainHand != null && OffHand == null &&
                       (int)(MainHand.WeaponType & WeaponType.Weapon2H) == 1;
            }
        }

        public IEnumerable<Armor> Armors
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

        #region Events
        public event EquipmentChangedEventHandler EquipmentChanged;
        public event WeaponChangedEventHandler WeaponChanged;
        #endregion

        public EquipmentContainer()
        {
        }

        public delegate void EquipmentChangedEventHandler(Equipment old, Equipment @new);
        public delegate void WeaponChangedEventHandler(Weapon old, Weapon @new, WeaponSlot slot);
    }
}
