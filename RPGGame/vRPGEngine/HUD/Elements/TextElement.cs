using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using vRPGEngine.HUD.Controls;
using System.Diagnostics;

namespace vRPGEngine.HUD.Elements
{
    public sealed class TextElement : IDisplayElement
    {
        #region Fields
        private Vector2 scale;
        private Vector2 position;

        private string text;

        private SpriteFont font;

        private Color textColor;
        #endregion

        public TextElement()
        {
        }

        public void Invalidate(Control control)
        {
            Debug.Assert(control != null);

            if (!control.ReadProperty("Text", ref text))             text      = string.Empty;
            if (!control.ReadProperty("Font", ref font))             font      = DefaultValues.DefaultFont;
            if (!control.ReadProperty("TextColor", ref textColor))   textColor = Color.White;
            
            if (string.IsNullOrEmpty(text)) return;

            var displaySize  = control.DisplaySize;
            var textSize     = font.MeasureString(text);
            position         = control.DisplayPosition;

            if (textSize == displaySize)
            {
                scale = Vector2.One;

                return;
            }

            var source = textSize;
            var target = displaySize;
            
            position = control.DisplayPosition;
            scale    = target / source;
        }

        public void Show(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (string.IsNullOrEmpty(text)) return;

            var textSize = font.MeasureString(text);

            spriteBatch.DrawString(font, 
                                   text, 
                                   position, 
                                   textColor, 
                                   0.0f, 
                                   Vector2.Zero, 
                                   scale, 
                                   SpriteEffects.None, 
                                   0.0f);
        }
    }
}
