using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vRPGContent.Data.Spells;

namespace vRPGContent.Data.Attributes
{
    [Serializable()]
    public sealed class SpecializationData
    {
        #region Properties
        public string Name
        {
            get;
            set;
        }
        public string Description
        {
            get;
            set;
        }
        public string TextureName
        {
            get;
            set;
        }
        public int[] Spells
        {
            get;
            set;
        }
        public int[] Buffs
        {
            get;
            set;
        }

        public int BaseStamina
        {
            get;
            set;
        }
        public int BaseIntellect
        {
            get;
            set;
        }
        public int BaseEndurance
        {
            get;
            set;
        }
        public int BaseAgility
        {
            get;
            set;
        }
        public int BaseStrength
        {
            get;
            set;
        }
        #endregion

        public SpecializationData()
        {
        }
    }
}
