using System;
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
        #region Fields
        private EquipmentContainer equipments;
        #endregion

        public Mage(EquipmentContainer equipments, AttributesData attributes, Statuses statuses) 
            : base(SpecializationDatabase.Instance.Elements().First(e => e.Name == "mage"), attributes, statuses)
        {
            this.equipments = equipments;
        }

         private bool IsWearingFullCloth()
        {
            return equipments.EquipedArmorsCount == equipments.Armors.Count(a => a.ArmorType == ArmorType.Cloth);
        }
        private bool IsWearingFullLeather()
        {
            return equipments.EquipedArmorsCount == equipments.Armors.Count(a => a.ArmorType == ArmorType.Leather);
        }

        public override float TotalCriticalHitPercent()
        {
            return base.TotalCriticalHitPercent();
        }
        
        public override int TotalSpellPower()
        {
            if (IsWearingFullCloth()) return (int)(base.TotalSpellPower() * 1.1f);

            return base.TotalSpellPower();
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
