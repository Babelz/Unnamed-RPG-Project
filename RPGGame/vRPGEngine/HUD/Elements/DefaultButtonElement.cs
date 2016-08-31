using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using vRPGEngine.HUD.Controls;
using System.Diagnostics;
using vRPGEngine.Core;

namespace vRPGEngine.HUD.Elements
{
    public sealed class AutonOsat
    {
        public string Nimi;
        public int Hinta;
    }
    public sealed class DefaultButtonElement : IDisplayElement
    {
        #region Fields
        private Vector2 textPosition;
        private Vector2 buttonPosition;
        private Vector2 scale;

        private string text;

        private Texture2D current;

        private SpriteFont font;

        private readonly Texture2D released;
        private readonly Texture2D pressed;
        #endregion

        public DefaultButtonElement()
        {
            released = Engine.Instance.Content.Load<Texture2D>("released");
            pressed  = Engine.Instance.Content.Load<Texture2D>("pressed"); ;

            font     = DefaultValues.DefaultFont;
            current  = released;
        }

        public void Invalidate(Control control)
        {
            Debug.Assert(control != null);

            ButtonControlState state = ButtonControlState.Up;

            if (control.ReadProperty("ButtonState", ref state))
            {
                if (state == ButtonControlState.Up || state == ButtonControlState.Released) current = pressed;
                else                                                                        current = released;
            }

            if (!control.ReadProperty("Text", ref text)) text = string.Empty;

            buttonPosition = control.DisplayPosition;

            var size                = control.DisplaySize;
            var currentTextureSize  = new Vector2(current.Width, current.Height);
            var textSize            = font.MeasureString(text);
            
            scale                   = size / currentTextureSize;
            textPosition            = buttonPosition - textSize / 2.0f + size / 2.0f;
        }

        public void Show(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(current,
                             buttonPosition,
                             null,
                             null,
                             null,
                             0.0f,
                             scale,
                             Color.White,
                             SpriteEffects.None,
                             0.0f);

            spriteBatch.DrawString(font, text, textPosition, Color.Black);
        }
    }
}
