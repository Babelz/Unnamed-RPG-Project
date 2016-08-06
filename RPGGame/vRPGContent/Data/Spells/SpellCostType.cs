using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vRPGContent.Data.Spells
{
    [Flags()]
    public enum SpellCostType : int
    {
        None            = 0,
        BasePercent     = 1,
        TotalPercent    = 2,
        Static          = 4
    }
}
