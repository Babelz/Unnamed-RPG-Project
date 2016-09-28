using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vRPGEngine.Core;

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

            margin.Top      = top * bounds.Height;
            margin.Bottom   = bottom * bounds.Height;
            margin.Left     = left * bounds.Width;
            margin.Right    = right * bounds.Width;

            return margin;
        }
    }
}
