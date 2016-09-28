using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vRPGEngine.Graphics;

namespace RPGGame.GUI
{
    public static class HUDConstants
    {
        #region Constant fields
        public static readonly string SampleTextLine    = "sample text line";
        public static readonly Vector2 IconSizeInPixels = new Vector2(32.0f);
        public static readonly Vector2 IconSize         = IconSizeInPixels / HUDRenderer.Instance.CanvasSize;

        public const int BuffIconColumns                = 10;
        public const int BuffIconRows                   = 3;        
        #endregion
    }
}
