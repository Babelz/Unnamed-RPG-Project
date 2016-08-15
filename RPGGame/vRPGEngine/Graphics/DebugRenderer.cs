using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vRPGEngine.Graphics
{
    public sealed class DebugRenderer : Singleton<DebugRenderer>
    {
        #region Fields
        private readonly List<Func<GameTime, string>> strings;

        private readonly SpriteBatch spriteBatch;

        private SpriteFont font;
        private Color color;
        #endregion

        private DebugRenderer()
            : base()
        {
            strings = new List<Func<GameTime, string>>();

            spriteBatch = new SpriteBatch(Engine.Instance.GraphicsDevice);

            font    = DefaultValues.DefaultFont;
            color   = Color.Red;
        }

        public void SetFont(SpriteFont font)
        {
            this.font = font == null ? DefaultValues.DefaultFont : font;
        }
        public void SetColor(Color color)
        {
            this.color = color;
        }

        public void AddString(Func<GameTime, string> func)
        {
            Debug.Assert(func != null);

            strings.Add(func);
        }
        
        public void Present(GameTime gameTime)
        {
            spriteBatch.Begin();

            var position = Vector2.Zero;
            var pad      = 16.0f;

            for (var i = 0; i < strings.Count; i++)
            { 
                var str = strings[i](gameTime);

                if (string.IsNullOrEmpty(str)) continue;

                var size = font.MeasureString(str);

                spriteBatch.DrawString(font, str, position, color);

                position.Y += size.Y + pad;
            }

            spriteBatch.End();
        }
    }
}
