using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vRPGData.Data.Enums;

namespace vRPGData.Data.Items
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
            Type        = ItemType.Consumable;
            EffectName  = string.Empty;
        }
    }
}
