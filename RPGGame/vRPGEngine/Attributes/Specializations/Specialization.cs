using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vRPGContent.Data.Attributes;
using vRPGContent.Data.Spells;
using vRPGEngine.Attributes;
using vRPGEngine.Databases;
using vRPGEngine.ECS.Components;

namespace vRPGEngine.Specializations
{
    /// <summary>
    /// Runtime specific computations regarding the attributes.
    /// </summary>
    public abstract class Specialization
    {
        #region Fields
        private readonly SpecializationData data;
        #endregion

        #region Properties
        public string Name
        {
            get
            {
                return data.Name;
            }
        }
        public string Description
        {
            get
            {
                return data.Description;
            }
        }

        /// <summary>
        /// Spell list.
        /// </summary>
        public IEnumerable<Spell> Spells
        {
            get
            {
                return SpellDatabase.Instance.Elements().Where(e => data.Spells.Contains(e.ID));
            }
        }
        /// <summary>
        /// Display elements.
        /// </summary>
        public IEnumerable<PassiveSpecializationBuff> Buffs
        {
            get
            {
                return PassiveSpecializationBuffDatabase.Instance.Elements().Where(e => data.Buffs.Contains(e.ID));
            }
        }
        #endregion

        public Specialization(SpecializationData data)
        {
            Debug.Assert(data != null);

            this.data = data;
        }

        public virtual int TotalAgility(AttributesData data)
        {
            return data.Agility;
        }
        public virtual int TotalArmor(AttributesData data)
        {
            return data.Armor;
        }
        public virtual float TotalBlockRatingPercent(AttributesData data)
        {
            return data.BlockRatingPercent;
        }
        public virtual float TotalCriticalHitPercent(AttributesData data)
        {
            return data.CriticalHitPercent;
        }
        public virtual float TotalDefenceRatingPercent(AttributesData data)
        {
            return data.DefenceRatingPercent;
        }
        public virtual float TotalDodgeRatingPercent(AttributesData data)
        {
            return data.DodgeRatingPercent;
        }
        public virtual int TotalEndurance(AttributesData data)
        {
            return data.Endurance;
        }
        public virtual int TotalFp5(AttributesData data)
        {
            return data.Fp5;
        }
        public virtual int TotalHaste(AttributesData data)
        {
            return data.Haste;
        }
        public virtual int TotalHp5(AttributesData data)
        {
            return data.Hp5;
        }
        public virtual int TotalIntellect(AttributesData data)
        {
            return data.Intellect;
        }
        public virtual float TotalMovementSpeedPercent(AttributesData data)
        {
            return data.MovementSpeedPercent;
        }
        public virtual int TotalMp5(AttributesData data)
        {
            return data.Mp5;
        }
        public virtual float TotalParryRatingPercent(AttributesData data)
        {
            return data.ParryRatingPercent;
        }
        public virtual int TotalStamina(AttributesData data)
        {
            return data.Stamina;
        }
        public virtual int TotalStrength(AttributesData data)
        {
            return data.Strength;
        }
    }
}
