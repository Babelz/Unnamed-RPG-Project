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
            var topLeftCorner       = new Rectangle(0, 0, left, top);
            var topRightCorner      = new Rectangle(texture.Width - right, 0, right, top);
            var bottomLeftCorner    = new Rectangle(0, texture.Height - bottom, left, bottom);
            var bottomRightCorner   = new Rectangle(texture.Width - right, texture.Height - bottom, right, bottom);

            var topPart             = new Rectangle(left, 0, texture.Width - left - right, top);
            var bottomPart          = new Rectangle(left, texture.Height - bottom, topPart.Width, bottom);
            var leftPart            = new Rectangle(0, top, left, texture.Height - bottom);
            var rightPart           = new Rectangle(texture.Width - right, top, right, texture.Height - bottom);
            var centerPart          = new Rectangle(left, top, texture.Width - left - right, texture.Height - top - bottom);

            yield return topLeftCorner;
            yield return topRightCorner;
            yield return bottomLeftCorner;
            yield return bottomRightCorner;

            yield return topPart;
            yield return bottomPart;
            yield return leftPart;
            yield return rightPart;
            yield return centerPart;
        }
    }
}
