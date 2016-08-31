using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using vRPGEngine.Graphics;
using vRPGEngine.HUD.Controls;
using System.Diagnostics;
using vRPGEngine.Core;

namespace vRPGEngine.HUD.Elements
{
    public sealed class SolidColorFill : IDisplayElement
    {
        #region Properties
        private Color   color;
        private Vector2 position;
        private Vector2 scale;
        #endregion

        public SolidColorFill()
        {
            color = Color.Red;
        }
        
        public void Invalidate(Control control)
        {
            Debug.Assert(control != null);

            var from      = new Vector2(DefaultValues.EmptyTexture.Width, DefaultValues.EmptyTexture.Height);
            var to        = control.DisplaySize;

            var min     = Vector2.Min(to, from);
            var max     = Vector2.Max(to, from);

            position    = control.DisplayPosition;
            scale       = max / min;

            if (!control.ReadProperty("Fill", ref color)) color = Color.Red;
        }

        public void Show(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(DefaultValues.EmptyTexture,
                             position,
                             null,
                             null,
                             null,
                             0.0f,
                             scale,
                             color,
                             SpriteEffects.None,
                             0.0f);
        }
    }
}
