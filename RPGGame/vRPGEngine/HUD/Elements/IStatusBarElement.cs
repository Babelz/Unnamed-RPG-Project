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
                Right   = new Rectangle(x + unitWidth * unitX, y, unitWidth, unitHeight)
            };
        }
    }

    public interface IStatusBarElement : IDisplayElement
    {
        void SetPresentationData(string texture, StatusBarTextureSources sources);
    }
}
