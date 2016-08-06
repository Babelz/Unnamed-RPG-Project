using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vRPGContent.Data.Spells
{
    [Flags()]
    public enum Currencies : int
    {
        None        = 0,
        Mana        = 1,
        Focus       = 2,
        ManaFocus   = Mana | Focus
    }
}
