using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vRPGEngine.Data
{
    [Serializable()]
    public sealed class Consumable : Item
    {
        #region Properties
        public string EffectName
        {
            get;
            set;
        }
        #endregion

        public Consumable()
            : base()
        {
            EffectName = string.Empty;
        }
    }
}
