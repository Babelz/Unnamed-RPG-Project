using Microsoft.Xna.Framework.Content.Pipeline;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using TiledSharp;

namespace vRPGContent.Tmx
{
    [ContentProcessor(DisplayName = "Tmx Processor")]
    public sealed class TmxProcessor : ContentProcessor<XDocument, TmxMap>
    {
        public override TmxMap Process(XDocument input, ContentProcessorContext context)
        {
            return new TmxMap(input);
        }
    }
}
