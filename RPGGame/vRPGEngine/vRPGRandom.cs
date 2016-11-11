using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            random = new Random(DateTime.Now.Millisecond);
        }

        public static int NextInt(int min, int max)
        {
            if (min == max) return min;

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
        public static float NextFloat(float min, float max)
        {
            Debug.Assert(min <= max);

            return (float)(min + (random.NextDouble() * (max - min)));
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

        public static Vector2 NextVector2(float minX, float maxX, float minY, float maxY)
        {
            return new Vector2(NextFloat(minX, maxX), NextFloat(minY, maxY));
        }

        public static Vector2 NextVector2(Vector2 min, Vector2 max)
        {
            return NextVector2(min.X, max.X, min.Y, max.Y);
        }
    }
}
