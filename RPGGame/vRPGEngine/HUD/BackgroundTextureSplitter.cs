using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vRPGEngine.HUD
{
    public sealed class BackgroundTextureSplitter
    {
        #region Fields
        private readonly int top;
        private readonly int bottom;
        private readonly int left;
        private readonly int right;
        #endregion
        
        public BackgroundTextureSplitter(int top, int bottom, int left, int right)
        {
            this.top        = top;
            this.bottom     = bottom;
            this.left       = left;
            this.right      = right;
        }
        public BackgroundTextureSplitter(int tblr)
            : this(tblr, tblr, tblr, tblr)
        {
        }

        public IEnumerable<Rectangle> Split(Texture2D texture)
        {
            var x               = 0;
            var y               = 0;
            var w               = texture.Width;
            var h               = texture.Height;

            var middleWidth     = w - left - right;
            var middleHeight    = h - top - bottom;
            var bottomY         = y + h - bottom;
            var rightX          = x + w - right;
            var leftX           = x + left;
            var topY            = y + top;

            yield return new Rectangle(x,      y,       left,  top);                    // top left
            yield return new Rectangle(leftX,  y,       middleWidth,  top);             // top middle
            yield return new Rectangle(rightX, y,       right, top);                    // top right
            yield return new Rectangle(x,      topY,    left,  middleHeight);           // left middle
            yield return new Rectangle(leftX,  topY,    middleWidth,  middleHeight);    // middle
            yield return new Rectangle(rightX, topY,    right, middleHeight);           // right middle
            yield return new Rectangle(x,      bottomY, left,  bottom);                 // bottom left
            yield return new Rectangle(leftX,  bottomY, middleWidth,  bottom);          // bottom middle
            yield return new Rectangle(rightX, bottomY, right, bottom);                 // bottom right
        }
    }
}
