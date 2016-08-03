using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vRPGEngine.Data
{
    [Flags()]
    public enum Categories : int
    {
        Tailoring = 0,
        LeatherWorking,
        Blacksmithing,
        Alchemy,
        Farming
    }
}
