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
    public abstract class Specialization
    {
        #region Constant fields
        public const int DefaultStaminaToHealthRation   = 10;
        public const int DefaultIntellectToManaRation   = 10;
        public const int DefaultEnduranceToFocusRation  = 10;
        #endregion

        #region Fields
        private readonly SpecializationData specialization;

        private readonly AttributesData attributes;

        private readonly Statuses statuses;

        private readonly int staminaToHealthRation;
        private readonly int intellectToManaRation;
        private readonly int enduranceToFocusRation;
        #endregion

        #region Properties
        protected AttributesData Attributes
        {
            get
            {
                return attributes;
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

        protected int IntellectToManaRation
        {
            get
            {
                return intellectToManaRation;
            }
        }
        protected int StaminaToHealthRation
        {
            get
            {
                return staminaToHealthRation;
            }
        }
        protected int EnduranceToFocusRation
        {
            get
            {
                return enduranceToFocusRation;
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

        protected Specialization(SpecializationData specialization, AttributesData attributes, Statuses statuses)
        {
            Debug.Assert(specialization != null);
            Debug.Assert(attributes != null);

            this.specialization  = specialization;
            this.attributes      = attributes;
            this.statuses        = statuses;

            SetRations(ref staminaToHealthRation, ref intellectToManaRation, ref enduranceToFocusRation);

            Debug.Assert(staminaToHealthRation != 0);
            Debug.Assert(intellectToManaRation != 0);
            Debug.Assert(enduranceToFocusRation != 0);
        }

        protected abstract void SetRations(ref int staminaToHealthRation, ref int intellectToManaRation, ref int enduranceToFocusRation);

        public virtual float CriticalDamagePercent()
        {
            // 200%.
            return 2.0f;
        }

        public virtual int TotalMana()
        {
            return TotalIntellect() * IntellectToManaRation;
        }

        public virtual int TotalAgility()
        {
            return attributes.Level * specialization.BaseAgility + attributes.Agility;
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
            return attributes.Level * specialization.BaseEndurance + attributes.Endurance;
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
            return attributes.Level * specialization.BaseIntellect + attributes.Intellect;
        }
        public virtual int TotalStamina()
        {
            return attributes.Level * specialization.BaseStamina + attributes.Stamina;
        }
        public virtual int TotalStrength()
        {
            return attributes.Level * specialization.BaseStrenght + attributes.Strength;
        }

        public virtual int TotalHealth()
        {
            return TotalStamina() * StaminaToHealthRation;
        }
        public virtual int TotalAttackPower()
        {
            return attributes.PureAttackPower;
        }
        public virtual int TotalSpellPower()
        {
            return attributes.PureSpellPower;
        }
        public virtual int TotalFocus()
        {
            return attributes.Endurance * EnduranceToFocusRation;
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

        public virtual int BaseHealth()
        {
            return specialization.BaseStamina * StaminaToHealthRation * attributes.Level;
        }
        public virtual int BaseMana()
        {
            return specialization.BaseIntellect * IntellectToManaRation * attributes.Level;
        }
        public virtual int BaseFocus()
        {
            return specialization.BaseEndurance * EnduranceToFocusRation * attributes.Level;
        }
    }
}
