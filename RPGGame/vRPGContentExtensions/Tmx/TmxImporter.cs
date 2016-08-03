using Microsoft.Xna.Framework.Content.Pipeline;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace vRPGContentExtensions.Tmx
{
    [ContentImporter(".tmx", DefaultProcessor = "TmxProcessor", DisplayName = "Tmx Importer")]
    public sealed class TmxImporter : ContentImporter<XDocument>
    {
        public override XDocument Import(string filename, ContentImporterContext context)
        {
            using (var reader = new StreamReader(filename))
            {
                return XDocument.Load(reader);
            }
        }
    }
}
