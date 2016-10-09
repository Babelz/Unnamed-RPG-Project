using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vRPGContent.Data.Attributes;
using vRPGContent.Data.Items.Enums;
using vRPGEngine;
using vRPGEngine.Attributes;
using vRPGEngine.Databases;
using vRPGEngine.Specializations;

namespace RPGGame.Specializations
{
    public sealed class Warrior : Specialization
    {
        public Warrior() 
            : base("warrior")
        {
        }

        #region Event handlers
        private void DamageController_OnSwing(ref MeleeSwingResults swing)
        {
            // Regenerate focus from melee swings. 
            // For example, if the warrior is level 20 and he deals 480 damage, he regenerates 24 focus.
            // Cricital hits always regenerate double the focus.
            var regen = swing.Damage / Attributes.Level;

            if (swing.Critical) regen *= 2;

            Statuses.Focus += regen;

            Statuses.Focus = Statuses.Focus >= TotalFocus() ? TotalFocus() : Statuses.Focus;

            if (regen != 0) GameInfoLog.Instance.LogGainFocus(regen, "melee swing", "Player");
        }
        #endregion

        protected override void SetRations(ref int staminaToHealthRation, ref int intellectToManaRation, ref int enduranceToFocusRation)
        {
            staminaToHealthRation  = DefaultStaminaToHealthRation;
            intellectToManaRation  = DefaultIntellectToManaRation;
            enduranceToFocusRation = 1;
        }

        private bool IsWearingFullPlate()
        {
            return Equipments.EquipedArmorsCount == Equipments.Armors.Count(a => a.ArmorType == ArmorType.Plate);
        }

        public override void Initialize(AttributesData attributes, Statuses statuses, EquipmentContainer equipments, MeleeDamageController meleeDamageController, RangedDamageController rangedDamageController)
        {
            base.Initialize(attributes, statuses, equipments, meleeDamageController, rangedDamageController);

            MeleeDamageController.OnSwing += DamageController_OnSwing;
        }

        public bool IsWearingFullMail()
        {
            return Equipments.EquipedArmorsCount == Equipments.Armors.Count(a => a.ArmorType == ArmorType.Mail);
        }
        public override int TotalStrength()
        {
            if (IsWearingFullMail()) return (int)(base.TotalStrength() * 0.1f);

            return Attributes.Strength;
        }
        public override int TotalStamina()
        {
            if (IsWearingFullPlate()) return (int)(base.TotalStamina() * 0.1f);

            return base.TotalStamina();
        }

        public override int TotalAttackPower()
        {
            // 2ap per 
            var pwr = (int)(base.TotalStrength() * 2.0f);

            return pwr + (int)(pwr * Attributes.AttackPowerPercentModifier) + Attributes.PureAttackPower;
        }
        public override int TotalHealth()
        {
            var stm = (int)(base.TotalStamina() * 2.0f);

            // 20% more health per point.
            stm += (int)(stm * 0.2f);

            // Base 10 health per stamina.
            return ((stm) + (int)(stm * Attributes.HealthPercentModifier)) * 10;
        }
        public override float DamageModifierPercent()
        {
            if (IsWearingFullMail()) return 0.1f;

            return 0.0f;
        }
        public override float TotalCriticalHitPercent()
        {
            if (IsWearingFullMail()) return Attributes.CriticalHitPercent + 0.05f;

            return Attributes.CriticalHitPercent;
        }
        public override int TotalFp5()
        {
            // As of the 75% reduction.
            return (int)Math.Round(Attributes.Fp5 * 0.25f, 0);
        }
        public override int TotalEndurance()
        {
            // Warriors have base endurance of 100 and max focus cap of 200.
            return (100 + base.TotalEndurance()) * EnduranceToFocusRation;
        }
        public override int TotalFocus()
        {
            var total = TotalEndurance();

            return total <= 200 ? total : 200;
        }
    }
}
