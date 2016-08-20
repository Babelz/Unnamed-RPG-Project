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

namespace vRPGEngine.HUD.Elements
{
    public struct TextLine
    {
        public Vector2 Position;
        public Vector2 Size;
        public string Contents;
    }

    public sealed class TextScrollElement : IScrollViewElement
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

        public TextScrollElement()
        {
            font            = DefaultValues.DefaultFont;
            spritebatch     = new SpriteBatch(Engine.Instance.GraphicsDevice);
            textColor       = Color.Black;
            backgroundColor = Color.White;
        }

        private IEnumerable<TextLine> GenerateLines()
        {
            var maxWidth             = size.X;
            var textSize             = font.MeasureString(text);
            var linesCount           = (int)Math.Round(textSize.X / maxWidth, 0);
            var lineWidth            = maxWidth / linesCount;
            var buffer               = new StringBuilder();
            var WidthOff             = 8.0f;
            var row                  = 0.0f;
            var rowOffset            = 0.0f;

            for (int i = 0; i < text.Length; i++)
            {
                var ch      = text[i];
                var str     = buffer.ToString();
                var strSize = font.MeasureString(str);

                if (strSize.X + WidthOff >= maxWidth)
                {
                    if      (ch == '.')         { buffer.Append(ch); }
                    else if (char.IsLetter(ch)) { buffer.Append('-'); i--; }
                    
                    TextLine line;

                    line.Position = new Vector2(0.0f, row + rowOffset);
                    line.Contents = buffer.ToString();
                    line.Size     = font.MeasureString(line.Contents);

                    row += line.Size.Y;

                    buffer.Clear();

                    yield return line;
                    
                    continue;
                }
                    
                buffer.Append(ch);
            }

            TextLine lastLine;
            lastLine.Position = new Vector2(0.0f, row + rowOffset);
            lastLine.Contents = buffer.ToString();
            lastLine.Size     = font.MeasureString(lastLine.Contents);

            if (!string.IsNullOrEmpty(lastLine.Contents)) yield return lastLine;
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
            position    = control.DisplayPosition;
            size        = control.DisplaySize;

            if (string.IsNullOrEmpty(text)) return;
            
            var lines = GenerateLines().ToList();

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