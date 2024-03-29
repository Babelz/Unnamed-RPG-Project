﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vRPGContent.Data.Attributes;
using vRPGContent.Data.Items.Enums;
using vRPGEngine.Attributes;
using vRPGEngine.Databases;
using vRPGEngine.Specializations;

namespace RPGGame.Specializations
{
    public sealed class Mage : Specialization
    {
        public Mage() 
            : base("mage")
        {
        }

        protected override void SetRations(ref int staminaToHealthRation, ref int intellectToManaRation, ref int enduranceToFocusRation)
        {
            staminaToHealthRation   = DefaultStaminaToHealthRation;
            intellectToManaRation   = DefaultIntellectToManaRation;
            enduranceToFocusRation  = DefaultEnduranceToFocusRation;
        }

        private bool IsWearingFullCloth()
        {
            return Equipments.EquipedArmorsCount == Equipments.Armors.Count(a => a.ArmorType == ArmorType.Cloth);
        }
        private bool IsWearingFullLeather()
        {
            return Equipments.EquipedArmorsCount == Equipments.Armors.Count(a => a.ArmorType == ArmorType.Leather);
        }

        public override float TotalCriticalHitPercent()
        {
            // Critical thinking.
            var critFromIntellect = (TotalIntellect() / (float)Attributes.Level) * 0.01f;

            return base.TotalCriticalHitPercent() + critFromIntellect;
        }
        
        public override int TotalSpellPower()
        {
            // Focused mind.
            var spellPowerFromIntellect = TotalIntellect() / 10;

            if (IsWearingFullCloth()) return (int)((base.TotalSpellPower() + spellPowerFromIntellect) * 1.1f);

            return base.TotalSpellPower() + spellPowerFromIntellect;
        }
        public override int TotalIntellect()
        {
            var mul = 1.0f;

            if      (IsWearingFullCloth())      mul = 1.1f;
            else if (IsWearingFullLeather())    mul = 1.05f;

            return (int)(base.TotalIntellect() * mul);
        }
        public override int TotalStamina()
        {
            if (IsWearingFullLeather()) return (int)(base.TotalStamina() * 1.1f);

            return base.TotalStamina();
        }
        public override float TotalMovementSpeedPercent()
        {
            if (IsWearingFullCloth()) return base.TotalMovementSpeedPercent() + 0.05f;

            return base.TotalMovementSpeedPercent();
        }
        public override int TotalHp5()
        {
            if (IsWearingFullLeather()) return base.TotalHp5() + Attributes.Level;

            return base.TotalHp5();
        }
    }
}
