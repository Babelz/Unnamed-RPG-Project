using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using vRPGEngine.HUD.Controls;
using vRPGContent.Data.Spells;
using vRPGContent.Data.Items;
using vRPGEngine.Handlers.Spells;
using System.Diagnostics;
using vRPGEngine.Graphics;
using vRPGEngine.Attributes.Spells;

namespace vRPGEngine.HUD.Elements
{
    public sealed class IconElement : IDisplayElement
    {
        #region Icon handlers
        private abstract class IconHandler
        {
            #region Properties
            protected Texture2D Background
            {
                get;
                set;
            }
            #endregion

            public IconHandler()
            {
                Background = Engine.Instance.Content.Load<Texture2D>("info background");
            }

            public abstract void Show(SpriteFont font, SpriteBatch spriteBatch, Vector2 position, Vector2 size, object content);
        }

        private sealed class BuffIconHandler : IconHandler
        {
            public BuffIconHandler()
                : base()
            {
            }

            public override void Show(SpriteFont font, SpriteBatch spriteBatch, Vector2 position, Vector2 size, object content)
            {
                var buff = content as Buff;

                Debug.Assert(buff != null);

                var header        = buff.FromSpell.Name;
                var lines         = TextHelper.GenerateLines(size, font, buff.FromSpell.Description).ToList();
                var mousePosition = HUDInputManager.Instance.MousePosition;
                var canvasSize    = HUDRenderer.Instance.CanvasSize;
                var textWidth     = lines.Max(l => l.Size.X);
                var textHeight    = lines.Max(l => l.Position.Y + l.Size.Y);
                var scale         = new Vector2(textWidth, textHeight) / new Vector2(Background.Width, Background.Height);

                spriteBatch.Draw(Background,
                                 mousePosition,
                                 null,
                                 null,
                                 null,
                                 0.0f,
                                 scale,
                                 Color.Wheat,
                                 SpriteEffects.None,
                                 0.0f);

                spriteBatch.DrawString(font, header, mousePosition, Color.White);

                foreach (var line in lines)
                {
                    spriteBatch.DrawString(font, line.Contents, line.Position + mousePosition, Color.White);
                }
            }
        }
        private sealed class ItemIconHandler : IconHandler
        {
            public ItemIconHandler()
                : base()
            {
            }

            public override void Show(SpriteFont font, SpriteBatch spriteBatch, Vector2 position, Vector2 size, object content)
            {
            }
        }
        private sealed class SpellIconHandler : IconHandler
        {
            public SpellIconHandler()
                : base()
            {
            }

            public override void Show(SpriteFont font, SpriteBatch spriteBatch, Vector2 position, Vector2 size, object content)
            {
            }
        }
        #endregion

        #region Fields
        private Vector2 position;
        private Vector2 size;

        private object content;
        #endregion

        public IconElement()
        {
        }

        public void SetContent(object content, Control sender)
        {
            Debug.Assert(content != null);
            Debug.Assert(sender != null);

            this.content = content;

            Invalidate(sender);
        }

        public void Invalidate(Control control)
        {
            Debug.Assert(content != null);

            position = control.DisplaySize;
            size     = HUDRenderer.Instance.CanvasSize * 0.1f;
        }

        public void Show(GameTime gameTime, SpriteBatch spriteBatch)
        {
            throw new NotImplementedException();
        }
    }
}
