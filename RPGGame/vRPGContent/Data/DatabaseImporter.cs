using Microsoft.Xna.Framework.Content.Pipeline;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace vRPGContent.Data
{
    [ContentImporter(".json", DefaultProcessor = "DefaultDatabaseProcessor", DisplayName = "RPG Database Importer")]
    public sealed class DatabaseImporter : ContentImporter<string>
    {
        public override string Import(string filename, ContentImporterContext context)
        {
            return filename;
        }
    }
}
