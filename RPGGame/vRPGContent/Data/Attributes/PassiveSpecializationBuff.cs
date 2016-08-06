using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vRPGContent.Data.Attributes
{
    [Serializable()]
    public sealed class PassiveSpecializationBuff
    {
        #region Properties
        public int ID
        {
            get;
            set;
        }
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
        public string IconName
        {
            get;
            set;
        }
        #endregion

        public PassiveSpecializationBuff()
        {
        }
    }
}
