using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vRPGEngine.HUD
{
    public struct Margin
    {
        #region Fields
        public float Top;
        public float Bottom;
        public float Left;
        public float Right;
        #endregion

        public static Margin Pixels(float top, float bottom, float left, float right)
        {
            Margin margin;

            margin.Top      = top;
            margin.Bottom   = bottom;
            margin.Left     = left;
            margin.Right    = right;

            return margin;
        }

        public static Margin Percents(Rectf bounds, float top, float bottom, float left, float right)
        {
            Margin margin;

            margin.Top      = top * bounds.H;
            margin.Bottom   = bottom * bounds.H;
            margin.Left     = left * bounds.W;
            margin.Right    = right * bounds.W;

            return margin;
        }
    }
}
