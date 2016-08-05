using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vRPGData.Data.Attributes;

namespace vRPGData.Data.Content
{
    class SpecializationDataReader : ContentTypeReader<SpecializationData[]>
    {
        protected override SpecializationData[] Read(ContentReader input, SpecializationData[] existingInstance)
        {
            var length  = input.ReadInt32();
            var bytes   = input.ReadBytes(length);

            return vRPGSerializer.GetObject(bytes) as SpecializationData[];
        }
    }
}
