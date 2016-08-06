using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content.Pipeline;

namespace vRPGContent.Data
{
    [ContentTypeWriter()]
    public sealed class DatabaseWriter : ContentTypeWriter<byte[]>
    {
        public override string GetRuntimeReader(TargetPlatform targetPlatform)
        {
            return "vRPGEngine.Content.SpecializationDataReader, vRPGEngine";
        }

        protected override void Write(ContentWriter output, byte[] value)
        {
            output.Write(value.Length);
            output.Write(value);
        }
    }
}
