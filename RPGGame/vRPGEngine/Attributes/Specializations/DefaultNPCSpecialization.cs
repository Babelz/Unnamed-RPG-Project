using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vRPGContent.Data.Attributes;
using vRPGContent.Data.Items.Enums;
using vRPGEngine.Databases;
using vRPGEngine.Specializations;

namespace vRPGEngine.Attributes.Specializations
{
    public sealed class DefaultNPCSpecialization : Specialization
    {
        public DefaultNPCSpecialization(AttributesData attributes, Statuses statuses)
            : base(SpecializationDatabase.Instance.Elements().First(e => e.Name.ToLower() == "npc default"), attributes, statuses)
        {
        }

        protected override void SetRations(ref int staminaToHealthRation, ref int intellectToManaRation, ref int enduranceToFocusRation)
        {
            staminaToHealthRation  = DefaultStaminaToHealthRation;
            intellectToManaRation  = DefaultIntellectToManaRation;
            enduranceToFocusRation = DefaultEnduranceToFocusRation;
        }
    }
}
