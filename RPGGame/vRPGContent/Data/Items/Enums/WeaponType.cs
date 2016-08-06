using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vRPGContent.Data.Enums
{
    [Flags()]
    public enum WeaponType : int
    {
        Weapon1H    = 1,
        Dagger      = 2 | Weapon1H,
        Sword       = 4 | Weapon1H,
        Axe         = 8 | Weapon1H,
        Mace        = 16 | Weapon1H,
        Weapon2H    = 32,
        Sword2H     = 64 | Weapon2H,
        Axe2H       = 128 | Weapon2H,
        Mace2H      = 256 | Weapon2H,
        Bow         = 512 | Weapon1H,
        Bow2H       = 1024 | Weapon2H,
        Gun         = 2048 | Weapon1H,
        Gun2H       = 4096 | Weapon2H,
        Shield      = 8192 | Weapon1H,
        OffHand     = 16384 | Weapon1H,
        Staff       = 32768 | Weapon2H
    }
}
