using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vRPGEngine.Readers
{
    public abstract class GenericElementReader<T> : ContentTypeReader<T[]>
    {
        protected override T[] Read(ContentReader input, T[] existingInstance)
        {
            var length = input.ReadInt32();
            var bytes  = input.ReadBytes(length);

            return vRPGSerializer.GetObject(bytes) as T[];
        }
    }
}
