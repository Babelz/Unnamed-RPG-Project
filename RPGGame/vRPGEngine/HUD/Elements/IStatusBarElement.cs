using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vRPGEngine.HUD.Elements
{
    public struct StatusBarTextureSources
    {
        #region Fields
        public Rectangle Left;
        public Rectangle Middle;
        public Rectangle Right;
        #endregion

        public static StatusBarTextureSources Compute(int x, int y, int unitWidth, int unitHeight, int unitX, int unitY)
        {
            return new StatusBarTextureSources()
            {
                Left    = new Rectangle(x, y, unitWidth, unitHeight),
                Middle  = new Rectangle(x + unitWidth, y, unitWidth * unitX, unitHeight * unitY),
                Right   = new Rectangle(x + unitWidth * (unitX + 1), y, unitWidth, unitHeight)
            };
        }
    }

    public struct StatusBarBindingsSource
    {
        #region Fields
        public StrongGetterDelegate<int> Min;
        public StrongGetterDelegate<int> Max;
        public StrongGetterDelegate<int> Value;
        #endregion

        public bool Bound()
        {
            return Min != null && Max != null && Value != null;
        }

        public static StatusBarBindingsSource Create(StrongGetterDelegate<int> min, StrongGetterDelegate<int> max, StrongGetterDelegate<int> value)
        {
            return new StatusBarBindingsSource()
            {
                Min     = min,
                Max     = max,
                Value   = value
            };
        }
    }

    public interface IStatusBarElement : IDisplayElement
    {
        void SetPresentationData(string texture, StatusBarTextureSources sources, StatusBarBindingsSource bindings);
    }
}
