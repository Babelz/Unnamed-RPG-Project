using Microsoft.Xna.Framework.Content.Pipeline;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vRPGContentExtensions.Data
{
    [ContentProcessor(DisplayName = "Default database processor")]
    public class DefaultDatabaseProcessor : ContentProcessor<string, object>
    {
        public override object Process(string input, ContentProcessorContext context)
        {
            context.Logger.LogWarning(string.Empty, null, "using default db processor!", null);

            return null;
        }
    }
}
