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
        private readonly SpecializationData data;

        private readonly int staminaToHealthRation;
        private readonly int intellectToManaRation;
        private readonly int enduranceToFocusRation;
        #endregion

        #region Properties
        protected AttributesData Attributes
        {
            get;
            private set;
        }
        protected Statuses Statuses
        {
            get;
            private set;
        }
        protected MeleeDamageController MeleeDamageController
        {
            get;
            private set;
        }
        protected RangedDamageController RangedDamageController
        {
            get;
            private set;
        }
        protected EquipmentContainer Equipments
        {
            get;
            private set;
        }

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

        protected Specialization(string name)
        {
            Debug.Assert(!string.IsNullOrEmpty(name));

            data = SpecializationDatabase.Instance.Elements().First(e => e.Name.ToLower() == name.ToLower());

            Debug.Assert(data != null);

            SetRations(ref staminaToHealthRation, ref intellectToManaRation, ref enduranceToFocusRation);
            
            Debug.Assert(staminaToHealthRation != 0);
            Debug.Assert(intellectToManaRation != 0);
            Debug.Assert(enduranceToFocusRation != 0);
        }

        protected abstract void SetRations(ref int staminaToHealthRation, ref int intellectToManaRation, ref int enduranceToFocusRation);

        public virtual void Initialize(AttributesData attributes, Statuses statuses, EquipmentContainer equipments, MeleeDamageController meleeDamageController, RangedDamageController rangedDamageController)
        {
            Debug.Assert(attributes != null);
            Debug.Assert(statuses != null);
            Debug.Assert(equipments != null);
            Debug.Assert(meleeDamageController != null);
            Debug.Assert(rangedDamageController != null);

            Attributes              = attributes;
            Statuses                = statuses;
            Equipments              = equipments;
            MeleeDamageController   = meleeDamageController;
            RangedDamageController  = rangedDamageController;
        }

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
            return Attributes.Level * data.BaseAgility + Attributes.Agility;
        }
        public virtual int TotalArmor()
        {
            return Attributes.Armor;
        }

        public virtual float TotalBlockRatingPercent()
        {
            return Attributes.BlockRatingPercent;
        }
        public virtual float TotalCriticalHitPercent()
        {
            return Attributes.CriticalHitPercent;
        }
        public virtual float TotalDefenceRatingPercent()
        {
            return Attributes.DefenceRatingPercent;
        }
        public virtual float TotalDodgeRatingPercent()
        {
            return Attributes.DodgeRatingPercent;
        }

        public virtual int TotalEndurance()
        {
            return Attributes.Level * data.BaseEndurance + Attributes.Endurance;
        }

        public virtual int TotalFp5()
        {
            return Attributes.Fp5;
        }
        public virtual int TotalHp5()
        {
            return Attributes.Hp5;
        }
        public virtual int TotalMp5()
        {
            return Attributes.Mp5;
        }

        public virtual float TotalMovementSpeedPercent()
        {
            return Attributes.MovementSpeedPercent;
        }
        public virtual int TotalHaste()
        {
            return Attributes.Haste;
        }

        public virtual float TotalParryRatingPercent()
        {
            return Attributes.ParryRatingPercent;
        }

        public virtual int TotalIntellect()
        {
            return Attributes.Level * data.BaseIntellect + Attributes.Intellect;
        }
        public virtual int TotalStamina()
        {
            return Attributes.Level * data.BaseStamina + Attributes.Stamina;
        }
        public virtual int TotalStrength()
        {
            return Attributes.Level * data.BaseStrength + Attributes.Strength;
        }

        public virtual int TotalHealth()
        {
            return TotalStamina() * StaminaToHealthRation;
        }
        public virtual int TotalAttackPower()
        {
            return Attributes.PureAttackPower;
        }
        public virtual int TotalSpellPower()
        {
            return Attributes.PureSpellPower;
        }
        public virtual int TotalFocus()
        {
            return Attributes.Endurance * EnduranceToFocusRation;
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
            return data.BaseStamina * StaminaToHealthRation * Attributes.Level;
        }
        public virtual int BaseMana()
        {
            return data.BaseIntellect * IntellectToManaRation * Attributes.Level;
        }
        public virtual int BaseFocus()
        {
            return data.BaseEndurance * EnduranceToFocusRation * Attributes.Level;
        }
    }
}
