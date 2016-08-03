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
        public static byte[] GetBytes(object value)
        {
            var bf = new BinaryFormatter();

            using (var ms = new MemoryStream())
            {
                bf.Serialize(ms, value);

                return ms.ToArray();
            }
        } 

        public static object GetObject(byte[] bytes)
        {
            using (var ms = new MemoryStream())
            {
                var bf = new BinaryFormatter();

                ms.Write(bytes, 0, bytes.Length);
                ms.Seek(0, SeekOrigin.Begin);

                var obj = bf.Deserialize(ms);

                return obj;
            }
        }

        public static void CopyMemory(ref object src, ref object dest, int size = 0)
        {
            var srcBytes    = GetBytes(src);
            var destBytes   = GetBytes(dest);

            var srcLen  = srcBytes.Length;
            var destLen = destBytes.Length;

            var len = size == 0 ? srcLen : size;

            if (len > destLen) throw new InvalidOperationException("dest.len < src.len");
            
            Array.Copy(srcBytes, destBytes, len);

            dest = GetObject(destBytes);
        }
    }
}
