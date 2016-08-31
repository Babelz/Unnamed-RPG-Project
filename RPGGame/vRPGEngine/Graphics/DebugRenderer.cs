using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vRPGEngine.Core;

namespace vRPGEngine.Graphics
{
    public sealed class DebugRenderer : Singleton<DebugRenderer>
    {
        #region Fields
        private readonly List<Func<Vector2[]>> points;
        private readonly List<Func<GameTime, string>> strings;

        private readonly SpriteBatch spriteBatch;

        private SpriteFont font;
        private Color color;

        private bool visible;
        #endregion

        private DebugRenderer()
            : base()
        {
            points  = new List<Func<Vector2[]>>();
            strings = new List<Func<GameTime, string>>();

            spriteBatch = new SpriteBatch(Engine.Instance.GraphicsDevice);

            font    = DefaultValues.DefaultFont;
            color   = Color.Red;
        }

        public void Toggle()
        {
            visible = !visible;
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

        public void AddPoints(Func<Vector2[]> func)
        {
            Debug.Assert(func != null);

            points.Add(func);
        }
        public void RemovePoints(Func<Vector2[]> func)
        {
            Debug.Assert(func != null);

            points.Remove(func);
        }

        public void Present(GameTime gameTime)
        {
            if (!visible) return;

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

            spriteBatch.Begin(transformMatrix: Renderer.Instance.Views().First().Transform);

            foreach (var func in points)
            {
                var points = func().Select(p => new Rectangle((int)p.X, (int)p.Y, 8, 8));

                foreach (var point in points) spriteBatch.Draw(DefaultValues.MissingTexture, point, color);
            }

            spriteBatch.End();
        }
    }
}
