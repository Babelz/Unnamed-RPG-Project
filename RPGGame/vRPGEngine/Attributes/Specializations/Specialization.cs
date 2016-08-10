using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vRPGContent.Data.Attributes;
using vRPGContent.Data.Items;
using vRPGContent.Data.Spells;
using vRPGEngine.Attributes;
using vRPGEngine.Databases;
using vRPGEngine.ECS.Components;

namespace vRPGEngine.Specializations
{
    /// <summary>
    /// Runtime specific computations regarding the attributes.
    /// </summary>
    public class Specialization
    {
        #region Fields
        private readonly SpecializationData specialization;

        private readonly AttributesData attributes;

        private readonly EquipmentContainer equipments;

        private readonly Statuses statuses;
        #endregion

        #region Properties
        protected AttributesData Attributes
        {
            get
            {
                return attributes;
            }
        }
        protected EquipmentContainer Equipments
        {
            get
            {
                return equipments;
            }
        }
        protected Statuses Statuses
        {
            get
            {
                return statuses;
            }
        }

        public string Name
        {
            get
            {
                return specialization.Name;
            }
        }
        public string Description
        {
            get
            {
                return specialization.Description;
            }
        }

        /// <summary>
        /// Spell list.
        /// </summary>
        public IEnumerable<Spell> Spells
        {
            get
            {
                return SpellDatabase.Instance.Elements().Where(e => specialization.Spells.Contains(e.ID));
            }
        }
        /// <summary>
        /// Display elements.
        /// </summary>
        public IEnumerable<PassiveSpecializationBuff> Buffs
        {
            get
            {
                return PassiveSpecializationBuffDatabase.Instance.Elements().Where(e => specialization.Buffs.Contains(e.ID));
            }
        }
        #endregion

        protected Specialization(SpecializationData specialization, AttributesData attributes, EquipmentContainer equipments, Statuses statuses)
        {
            Debug.Assert(specialization != null);
            Debug.Assert(attributes != null);

            this.specialization  = specialization;
            this.attributes      = attributes;
            this.equipments      = equipments;
            this.statuses        = statuses;
        }

        public virtual float CriticalDamagePercent()
        {
            // 200%.
            return 2.0f;
        }

        public virtual int TotalMana()
        {
            return attributes.Intellect * 10;
        }

        public virtual int TotalAgility()
        {
            return attributes.Agility;
        }
        public virtual int TotalArmor()
        {
            return attributes.Armor;
        }

        public virtual float TotalBlockRatingPercent()
        {
            return attributes.BlockRatingPercent;
        }
        public virtual float TotalCriticalHitPercent()
        {
            return attributes.CriticalHitPercent;
        }
        public virtual float TotalDefenceRatingPercent()
        {
            return attributes.DefenceRatingPercent;
        }
        public virtual float TotalDodgeRatingPercent()
        {
            return attributes.DodgeRatingPercent;
        }

        public virtual int TotalEndurance()
        {
            return attributes.Endurance;
        }

        public virtual int TotalFp5()
        {
            return attributes.Fp5;
        }
        public virtual int TotalHp5()
        {
            return attributes.Hp5;
        }
        public virtual int TotalMp5()
        {
            return attributes.Mp5;
        }

        public virtual float TotalMovementSpeedPercent()
        {
            return attributes.MovementSpeedPercent;
        }
        public virtual int TotalHaste()
        {
            return attributes.Haste;
        }

        public virtual float TotalParryRatingPercent()
        {
            return attributes.ParryRatingPercent;
        }

        public virtual int TotalIntellect()
        {
            return attributes.Intellect;
        }
        public virtual int TotalStamina()
        {
            return attributes.Stamina;
        }
        public virtual int TotalStrength()
        {
            return attributes.Strength;
        }

        public virtual int TotalHealth()
        {
            return TotalStamina() * 10;
        }
        public virtual int TotalMeleePower()
        {
            return attributes.PureMeleePower;
        }
        public virtual int TotalSpellPower()
        {
            return attributes.PureSpellPower;
        }
        public virtual int TotalFocus()
        {
            return attributes.Endurance * 10;
        }

        public virtual float MeleeDamageModifierPercent()
        {
            return 0.0f;
        }
        public virtual float SpellDamageModifierPercent()
        {
            return 0.0f;
        }
        public virtual float DamageModifierPercent()
        {
            return 0.0f;
        }
    }
}
