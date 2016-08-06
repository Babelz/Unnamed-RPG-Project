using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiledSharp;

namespace vRPGEngine.Readers
{
    public sealed class TmxReader : ContentTypeReader<TmxMap>
    {
        protected override TmxMap Read(ContentReader input, TmxMap existingInstance)
        {
            var length  = input.ReadInt32();
            var bytes   = input.ReadBytes(length);

            return vRPGSerializer.GetObject(bytes) as TmxMap;
        }
    }
}
