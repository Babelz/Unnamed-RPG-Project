using Microsoft.Xna.Framework.Content;
using vRPGData.Data.Attributes;

namespace vRPGData.Data.Content
{
    public sealed class SpecializationDataReader : ContentTypeReader<SpecializationData[]>
    {
        protected override SpecializationData[] Read(ContentReader input, SpecializationData[] existingInstance)
        {
            var length  = input.ReadInt32();
            var bytes   = input.ReadBytes(length);

            return vRPGSerializer.GetObject(bytes) as SpecializationData[];
        }
    }
}
