using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace vRPGEngine
{
    public static class vRPGSerializer
    {
        public static byte[] GetBytes(object @object)
        {
            var binaryFormatter = new BinaryFormatter();

            using (var ms = new MemoryStream())
            {
                binaryFormatter.Serialize(ms, @object);

                return ms.ToArray();
            }
        } 

        public static object GetObject(byte[] bytes)
        {
            using (var memoryStream = new MemoryStream())
            {
                var binaryFormatter = new BinaryFormatter();

                memoryStream.Write(bytes, 0, bytes.Length);
                memoryStream.Seek(0, SeekOrigin.Begin);

                var @object = binaryFormatter.Deserialize(memoryStream);

                return @object;
            }
        }

        public static void CopyMemory(ref object source, ref object destionation, int size = 0)
        {
            var sourceBytes         = GetBytes(source);
            var destionationBytes   = GetBytes(destionation);

            var sourceLength        = sourceBytes.Length;
            var destionationLength  = destionationBytes.Length;

            var length = size == 0 ? sourceLength : size;

            if (length > destionationLength) throw new InvalidOperationException("sizeof(destionation) < sizeof(source)");
            
            Array.Copy(sourceBytes, destionationBytes, length);

            destionation = GetObject(destionationBytes);
        }
    }
}
