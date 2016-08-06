using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vRPGContent.Data.Enums
{
    [Flags()]
    public enum Elements : int
    {
        None    = 0,
        Holy    = 1,
        Dark    = 2,
        Eath    = 4,
        Fire    = 8,
        Wind    = 16,
        Frost   = 32,
        Water   = 64,
        Nature  = 128,
        Magical = 256,
        Void    = 512
    }
}
