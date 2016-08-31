using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using vRPGEngine.HUD.Controls;
using vRPGEngine.HUD.Interfaces;
using System.IO;
using System.Diagnostics;
using vRPGEngine.Core;

namespace vRPGEngine.HUD.Elements
{
    public sealed class TextScrollViewElement : IScrollViewElement
    {
        #region Fields
        private SpriteBatch spritebatch;
        private RenderTarget2D renderTarget;
        private SpriteFont font;

        private Vector2 overlap;
        private Vector2 size;
        private Vector2 offset;
        private Vector2 position;

        private string text;

        private Color textColor;
        private Color backgroundColor;
        #endregion

        #region Properties        
        public SpriteFont Font
        {
            get
            {
                return font;
            }
            set
            {
                font = value;

                if (font == null) font = DefaultValues.DefaultFont;
            }
        }

        public string Text
        {
            get
            {
                return text;
            }
            set
            {
                text = value;
            }
        }

        public Vector2 Overlap
        {
            get
            {
                return overlap;
            }
        }

        public Color TextColor
        {
            get
            {
                return textColor;
            }
            set
            {
                textColor = value;
            }
        }
        public Color BackgroundColor
        {
            get
            {
                return backgroundColor;
            }
            set
            {
                backgroundColor = value;
            }
        }
        #endregion

        public TextScrollViewElement()
        {
            font            = DefaultValues.DefaultFont;
            spritebatch     = new SpriteBatch(Engine.Instance.GraphicsDevice);
            textColor       = Color.Black;
            backgroundColor = Color.White;
        }

        private void InvalidateRenderTarget(int width, int height)
        {
            if (renderTarget == null)
            {
                renderTarget = new RenderTarget2D(Engine.Instance.GraphicsDevice, width, height);
            }
            else if (renderTarget.Width != width || renderTarget.Height != height)
            {
                renderTarget.Dispose();

                renderTarget = new RenderTarget2D(Engine.Instance.GraphicsDevice, width, height);
            }
        }

        private void DrawToRenderTarget(IEnumerable<TextLine> lines)
        {
            var device = Engine.Instance.GraphicsDevice;

            device.SetRenderTarget(renderTarget);
            device.Clear(BackgroundColor);

            spritebatch.Begin();

            foreach (var line in lines) spritebatch.DrawString(font, line.Contents, line.Position, TextColor);

            spritebatch.End();

            device.SetRenderTarget(null);
        }

        public void SetScroll(Vector2 scroll, Control sender)
        {
            offset = scroll;

            Invalidate(sender);
        }

        public void Invalidate(Control control)
        {
            Debug.Assert(control != null);

            position    = control.DisplayPosition;
            size        = control.DisplaySize;

            if (string.IsNullOrEmpty(text)) return;

            var lines = TextHelper.GenerateLines(size, font, text).ToList();

            var width  = (int)lines.Max(l => l.Size.X);
            var height = (int)lines.Max(l => l.Position.Y + l.Size.Y);

            InvalidateRenderTarget(width, height);

            DrawToRenderTarget(lines);

            var targetSize = new Vector2(renderTarget.Width, renderTarget.Height);

            overlap = targetSize - size;

            if (overlap.X < 0.0f) overlap.X = 0.0f;
            if (overlap.Y < 0.0f) overlap.Y = 0.0f;
        }

        public void Show(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (renderTarget == null)
            {
                spriteBatch.Draw(DefaultValues.MissingTexture, new Rectangle((int)position.X, (int)position.Y, (int)size.X, (int)size.Y), Color.White);
            }
            else
            {
                Rectangle src = new Rectangle((int)offset.X, (int)offset.Y, (int)size.X, (int)size.Y);

                spriteBatch.Draw(renderTarget,
                                 position,
                                 null,
                                 src,
                                 null,
                                 0.0f,
                                 null,
                                 Color.White,
                                 SpriteEffects.None,
                                 0.0f);
            }
        }
    }
}