using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vRPGEngine.Attributes
{
    /// <summary>
    /// Runtime specific attributes of some entity.
    /// </summary>
    public sealed class AttributesData 
    {
        #region Properties
        public int Armor
        {
            get;
            set;
        }
        public int Stamina
        {
            get;
            set;
        }
        public int Intellect
        {
            get;
            set;
        }
        public int Endurance
        {
            get;
            set;
        }
        public int Strength
        {
            get;
            set;
        }
        public int Agility
        {
            get;
            set;
        }
        public int Mp5
        {
            get;
            set;
        }
        public int Fp5
        {
            get;
            set;
        }
        public int Hp5
        {
            get;
            set;
        }
        public int Haste
        {
            get;
            set;
        }
        public float CriticalHitPercent
        {
            get;
            set;
        }
        public float DefenceRatingPercent
        {
            get;
            set;
        }
        public float BlockRatingPercent
        {
            get;
            set;
        }
        public float DodgeRatingPercent
        {
            get;
            set;
        }
        public float ParryRatingPercent
        {
            get;
            set;
        }
        public float MovementSpeedPercent
        {
            get;
            set;
        }
        #endregion

        public AttributesData()
        {
        }
    }
}
