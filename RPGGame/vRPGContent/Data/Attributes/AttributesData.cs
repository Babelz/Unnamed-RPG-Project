﻿using System;
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
        #region Properties
        public int Level
        {
            get;
            set;
        }
        public int XP
        {
            get;
            set;
        }

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
        public int PureMeleePower
        {
            get;
            set;
        }
        public int PureSpellPower
        {
            get;
            set;
        }

        public float HealthPercentModifier
        {
            get;
            set;
        }
        public float MeeleePowerPercentModifier
        {
            get;
            set;
        }
        public float FocusPercentModifier
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
