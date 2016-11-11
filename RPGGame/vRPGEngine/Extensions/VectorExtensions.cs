using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vRPGEngine.Extensions
{
    public static class VectorExtensions
    {
        public static bool AlmostEqual(Vector2 a, Vector2 b)
        {
            var ax = (int)a.X;
            var ay = (int)a.Y;

            var bx = (int)b.X;
            var by = (int)b.Y;

            return ax == bx && ay == by;
        }

        public static bool Intersects(Vector2 aPosition, Vector2 aSize, Vector2 bPosition, Vector2 bSize)
        {
            var aLeft       = aPosition.X;
            var aRight      = aPosition.X + aSize.X;
            var aTop        = aPosition.Y;
            var aBottom     = aPosition.Y + aSize.Y;

            var bLeft       = bPosition.X;
            var bRight      = bPosition.X + bSize.X;
            var bTop        = bPosition.Y;
            var bBottom     = bPosition.Y + bSize.Y;

            return aLeft < bRight &&
                   bLeft < aRight &&
                   aTop < bBottom &&
                   bTop < aBottom;
        }

        public static Vector2 Abs(this Vector2 vector)
        {
            Vector2 results;
            
            results.X = Math.Abs(vector.X);
            results.Y = Math.Abs(vector.Y);

            return results;
        }
    }
}
