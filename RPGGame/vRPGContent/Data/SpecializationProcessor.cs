using Microsoft.Xna.Framework.Content.Pipeline;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using vRPGContent.Data.Attributes;

namespace vRPGContent.Data
{
    [ContentProcessor(DisplayName = "Specialization processor")]
    public sealed class SpecializationProcessor : ContentProcessor<string, SpecializationData[]>
    {
        public override SpecializationData[] Process(string input, ContentProcessorContext context)
        {
            JsonSerializer serializer   = new JsonSerializer();
            var contents                = File.ReadAllText(input);
            var results                 = JsonConvert.DeserializeObject<DataCollection<SpecializationData>>(contents);

            return results.data;
        }
    }
}
