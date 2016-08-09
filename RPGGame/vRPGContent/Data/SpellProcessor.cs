using Microsoft.Xna.Framework.Content.Pipeline;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vRPGContent.Data.Spells;

namespace vRPGContent.Data
{
    [ContentProcessor(DisplayName = "Spell processor")]
    public sealed class SpellProcessor : GenericElementProcessor<Spell>
    {
    }
}
