using Microsoft.Xna.Framework.Content.Pipeline;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vRPGContent.Data.Characters;

namespace vRPGContent.Data
{
    [ContentProcessor(DisplayName = "NPC processor")]
    public sealed class NPCProcessor : GenericElementProcessor<NPCData>
    {
    }
}
