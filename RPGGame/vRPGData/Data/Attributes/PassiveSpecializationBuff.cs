using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vRPGData.Data.Attributes
{
    [Serializable()]
    public sealed class PassiveSpecializationBuff
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
        #endregion

        public PassiveSpecializationBuff()
        {
        }
    }
}
