using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using vRPGEngine.Graphics;

namespace vRPGEngine.HUD
{
    public sealed class SolidColorFill : IDisplayElement
    {
        #region Properties
        private Vector2 position;
        private Vector2 scale;
        #endregion

        #region Properties  
        public Color Color
        {
            get;
            set;
        }
        #endregion

        public SolidColorFill()
        {
            Color = Color.Red;
        }
        
        public void Invalidate(Control control)
        {
            var to      = new Vector2(DefaultValues.EmptyTexture.Width, DefaultValues.EmptyTexture.Height);
            var from    = control.DisplayPosition;

            var min     = Vector2.Min(to, from);
            var max     = Vector2.Max(to, from);

            position    = control.DisplayPosition;
            scale       = min / max;
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
                             Color,
                             SpriteEffects.None,
                             0.0f);
        }
    }
}
