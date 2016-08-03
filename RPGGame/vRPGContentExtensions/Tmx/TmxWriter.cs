using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TiledSharp;
using Microsoft.Xna.Framework.Content.Pipeline;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace vRPGContentExtensions.Tmx
{
    [ContentTypeWriter()]
    public sealed class TmxWriter : ContentTypeWriter<TmxMap>
    {
        public override string GetRuntimeReader(TargetPlatform targetPlatform)
        {
            return "vRPGEngine.Maps.Content.TmxReader, vRPGEngine";
        }

        protected override void Write(ContentWriter output, TmxMap value)
        {
            var bytes = vRPGSerializer.GetBytes(value);

            output.Write(bytes.Length);
            output.Write(bytes);
        }
    }
}
