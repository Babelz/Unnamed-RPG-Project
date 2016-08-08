using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vRPGContent.Data.Items.Enums
{
    [Flags()]
    public enum Categories : int
    {
        Tailoring       = 1,
        LeatherWorking  = 2,
        Blacksmithing   = 4,
        Alchemy         = 8,
        Farming         = 16
    }
}
