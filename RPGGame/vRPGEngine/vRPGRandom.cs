using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vRPGEngine
{
    public static class vRPGRandom
    {
        #region Fields
        private static readonly Random random;
        #endregion

        static vRPGRandom()
        {
            random = new Random();
        }

        public static int NextInt(int min, int max)
        {
            return random.Next(min, max);
        }
        public static int NextInt(int max)
        {
            return random.Next(max);
        }
        public static float NextFloat()
        {
            return (float)random.NextDouble();
        }
        public static double NextDouble()
        {
            return random.NextDouble();
        }
        public static byte[] NextBytes(int count)
        {
            var bytes = new byte[count];

            random.NextBytes(bytes);

            return bytes;
        }
    }
}
