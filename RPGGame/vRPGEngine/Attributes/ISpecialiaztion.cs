using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vRPGEngine.Attributes;
using vRPGEngine.ECS.Components;

namespace vRPGEngine.Attributes
{
    /// <summary>
    /// Runtime specific computations regarding the attributes.
    /// </summary>
    public interface ISpecialiaztion
    {
        int TotalArmor(AttributesData data);
        int TotalStamina(AttributesData data);
        int TotalIntellect(AttributesData data);
        int TotalEndurance(AttributesData data);
        int TotalStrength(AttributesData data);
        int TotalAgility(AttributesData data);
        int TotalMp5(AttributesData data);
        int TotalEp5(AttributesData data);
        int TotalHp5(AttributesData data);
        int TotalHaste(AttributesData data);
        float TotalCriticalHitPercent(AttributesData data);
        float TotalDefenceRatingPercent(AttributesData data);
        float TotalBlockRatingPercent(AttributesData data);
        float TotalDodgeRatingPercent(AttributesData data);
        float TotalParryRatingPercent(AttributesData data);
        float TotalMovementSpeedPercent(AttributesData data);
    }
}
