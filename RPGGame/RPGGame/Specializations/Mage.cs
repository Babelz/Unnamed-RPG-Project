using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vRPGContent.Data.Attributes;
using vRPGEngine.Attributes;
using vRPGEngine.Databases;
using vRPGEngine.Specializations;

namespace RPGGame.Specializations
{
    public sealed class Mage : Specialization
    {
        public Mage(AttributesData attributes, Statuses statuses) 
            : base(SpecializationDatabase.Instance.Elements().First(e => e.Name == "mage"), attributes, statuses)
        {
            
        }
    }
}
