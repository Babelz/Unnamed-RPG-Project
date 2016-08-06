using Microsoft.Xna.Framework.Content.Pipeline;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vRPGContent.Data.Attributes;

namespace vRPGContent.Data
{
    [ContentProcessor(DisplayName = "Spec passive buff processor")]
    public sealed class SpecPassiveBuffProcessor : GenericElementProcessor<PassiveSpecializationBuff>
    {
    }
}
