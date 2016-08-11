using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vRPGContent.Data.Attributes;
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
    }
}
