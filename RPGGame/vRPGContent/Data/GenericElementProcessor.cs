using Microsoft.Xna.Framework.Content.Pipeline;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vRPGContent.Data
{
    public abstract class GenericElementProcessor<T> : ContentProcessor<string, T[]> where T : class
    {
        public override T[] Process(string input, ContentProcessorContext context)
        {
            JsonSerializer serializer = new JsonSerializer();

            var lines       = File.ReadAllLines(input);
            lines           = vJson.RemoveComments(lines);

            var contents    = vJson.Join(lines);
            var results     = JsonConvert.DeserializeObject<DataCollection<T>>(contents);

            return results.data;
        }
    }
}
