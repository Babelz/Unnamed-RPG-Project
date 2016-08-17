using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vRPGEngine.HUD
{
    public struct Padding
    {
        #region Fields
        public float Top;
        public float Bottom;
        public float Left;
        public float Right;
        #endregion

        public static Padding Pixels(float top, float bottom, float left, float right)
        {
            Padding padding;

            padding.Top     = top;
            padding.Bottom  = bottom;
            padding.Left    = left;
            padding.Right   = right;

            return padding;
        }

        public static Padding Percents(Rectf bounds, float top, float bottom, float left, float right)
        {
            Padding padding;

            padding.Top     = top * bounds.H;
            padding.Bottom  = bottom * bounds.H;
            padding.Left    = left * bounds.W;
            padding.Right   = right * bounds.W;
            
            return padding;
        }
    }
}
