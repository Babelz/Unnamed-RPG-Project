using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vRPGContent.Data.Attributes;
using vRPGEngine.Specializations;

namespace vRPGEngine.Attributes.Specializations
{
    public sealed class Warrior : Specialization
    {
        public Warrior(SpecializationData data) 
            : base(data)
        {
        }
    }
}
