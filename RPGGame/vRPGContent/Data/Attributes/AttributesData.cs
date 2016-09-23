using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vRPGContent.Data.Attributes
{
    /// <summary>
    /// Runtime specific attributes of some entity.
    /// </summary>
    [Serializable()]
    public class AttributesData 
    {
        #region Fields
        private int level;
        private int xp;

        private int armor;

        private int stamina;
        private int intellect;
        private int endurance;
        private int strength;
        private int agility;

        private int mp5;
        private int fp5;
        private int hp5;

        private int haste;

        private float criticalHitPercent;
        private float defenceRatingPercent;
        private float blockRatingPercent;
        private float dodgeRatingPercent;
        private float parryRatingPercent;
        private float movementSpeedPercent;

        private int pureMeleePower;
        private int pureSpellPower;

        private float healthPercentModifier;
        private float meleePowerPercentModifier;
        private float focusPercentModifier;
        #endregion

        #region Events
        // TODO: impl to properties.
        public event AttributeChangedEventHandler AttributeChanged;
        #endregion

        #region Properties
        public int Level
        {
            get
            {
                return level;
            }
            set
            {
                level = value;
            }
        }
        public int Xp
        {
            get
            {
                return xp;
            }
            set
            {
                xp = value;
            }
        }

        public int Armor
        {
            get
            {
                return armor;
            }
            set
            {
                armor = value;
            }
        }

        public int Stamina
        {
            get
            {
                return stamina;
            }
            set
            {
                stamina = value;
            }
        }
        public int Intellect
        {
            get
            {
                return intellect;
            }
            set
            {
                intellect = value;
            }
        }
        public int Endurance
        {
            get
            {
                return endurance;
            }
            set
            {
                endurance = value;
            }
        }
        public int Strength
        {
            get
            {
                return strength;
            }
            set
            {
                strength = value;
            }
        }
        public int Agility
        {
            get
            {
                return agility;
            }
            set
            {
                agility = value;
            }
        }

        public int Mp5
        {
            get
            {
                return mp5;
            }
            set
            {
                mp5 = value;
            }
        }
        public int Fp5
        {
            get
            {
                return fp5;
            }
            set
            {
                fp5 = value;
            }
        }
        public int Hp5
        {
            get
            {
                return hp5;
            }
            set
            {
                hp5 = value;
            }
        }

        public int Haste
        {
            get
            {
                return haste;
            }
            set
            {
                haste = value;
            }
        }

        public float CriticalHitPercent
        {
            get
            {
                return criticalHitPercent;
            }
            set
            {
                criticalHitPercent = value;
            }
        }
        public float DefenceRatingPercent
        {
            get
            {
                return defenceRatingPercent;
            }
            set
            {
                defenceRatingPercent = value;
            }
        }
        public float BlockRatingPercent
        {
            get
            {
                return blockRatingPercent;
            }
            set
            {
                blockRatingPercent = value;
            }
        }
        public float DodgeRatingPercent
        {
            get
            {
                return dodgeRatingPercent;
            }
            set
            {
                dodgeRatingPercent = value;
            }
        }
        public float ParryRatingPercent
        {
            get
            {
                return parryRatingPercent;
            }
            set
            {
                parryRatingPercent = value;
            }
        }
        public float MovementSpeedPercent
        {
            get
            {
                return movementSpeedPercent;
            }
            set
            {
                movementSpeedPercent = value;
            }
        }

        public int PureMeleePower
        {
            get
            {
                return pureMeleePower;
            }
            set
            {
                pureMeleePower = value;
            }
        }
        public int PureSpellPower
        {
            get
            {
                return pureSpellPower;
            }
            set
            {
                pureSpellPower = value;
            }
        }

        public float HealthPercentModifier
        {
            get
            {
                return healthPercentModifier;
            }
            set
            {
                healthPercentModifier = value;
            }
        }
        public float MeleePowerPercentModifier
        {
            get
            {
                return meleePowerPercentModifier;
            }
            set
            {
                meleePowerPercentModifier = value;
            }
        }
        public float FocusPercentModifier
        {
            get
            {
                return focusPercentModifier;
            }
            set
            {
                focusPercentModifier = value;
            }
        }
        #endregion
        
        public AttributesData()
        {
        }
    }

    public delegate void AttributeChangedEventHandler(string name, object value);
}
