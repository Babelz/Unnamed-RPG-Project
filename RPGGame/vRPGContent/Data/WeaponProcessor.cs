﻿using Microsoft.Xna.Framework.Content.Pipeline;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vRPGContent.Data.Items;

namespace vRPGContent.Data
{
    [ContentProcessor(DisplayName = "Weapon processor")]
    public sealed class WeaponProcessor : GenericElementProcessor<Weapon>
    {
    }
}
