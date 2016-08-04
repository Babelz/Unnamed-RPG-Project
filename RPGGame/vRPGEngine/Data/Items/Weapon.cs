using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vRPGEngine.Data.Enums;

namespace vRPGEngine.Data.Items
{
    [Serializable()]
    public sealed class Weapon : Item
    {
        #region Properties
        public int SwingTimer
        {
            get;
            set;
        }
        public int BaseDamageMin
        {
            get;
            set;
        }
        public int BaseDamageMax
        {
            get;
            set;
        }
        public Elements ElementalDamageType
        {
            get;
            set;
        }
        public int ElementDamageMin
        {
            get;
            set;
        }
        public int ElementalDamageMax
        {
            get;
            set;
        }
        public string SoundsSet
        {
            get;
            set;
        }
        public WeaponType WeaponType
        {
            get;
            set;
        }
        #endregion

        public Weapon()
            : base()
        {
            SoundsSet   = string.Empty;
            Type        = ItemType.Weapon;
        }
    }
}
