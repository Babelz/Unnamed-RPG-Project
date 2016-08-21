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
        private abstract class IconElementHandler
        {
            #region Properties
            protected Texture2D Icon
            {
                get;
                set;
            }
            protected Texture2D Background
            {
                get;
                set;
            }
            #endregion

            public IconElementHandler()
            {
                Background = Engine.Instance.Content.Load<Texture2D>("info background");
            }

            public abstract void Show(SpriteFont font, SpriteBatch spriteBatch, Vector2 position, Vector2 size, MouseHoverState hoverState);
        }

        private sealed class BuffIconHandler : IconElementHandler
        {
            #region Fields
            private readonly BackgroundTextureSplitter splitter;
            private readonly SelfBuffSpellHandler handler;
            #endregion

            public BuffIconHandler(SelfBuffSpellHandler handler)
                : base()
            {
                Debug.Assert(handler != null);

                this.handler = handler;

                Icon         = Engine.Instance.Content.Load<Texture2D>(handler.Spell.IconName);
                splitter = new BackgroundTextureSplitter(3);
            }

            public override void Show(SpriteFont font, SpriteBatch spriteBatch, Vector2 position, Vector2 size, MouseHoverState hoverState)
            {
                var iconScale   = size;
                iconScale.X     /= Icon.Width;
                iconScale.Y     /= Icon.Height;
                
                foreach (var part in splitter.Split(Icon))
                {
                    var partPosition     = new Vector2(part.X * iconScale.X, part.Y * iconScale.Y) + position;
                    var partSize         = new Vector2(part.Width * iconScale.X, part.Height * iconScale.Y);
                    var partDestionation = new Rectangle((int)partPosition.X, (int)partPosition.Y, (int)partSize.X, (int)partSize.Y);

                    spriteBatch.Draw(Icon, partDestionation, part, Color.White);
                }

                if (hoverState == MouseHoverState.Hover)
                {
                    const float ElementOffset = 16.0f;

                    var header          = handler.Spell.Name;
                    var canvasSize      = HUDRenderer.Instance.CanvasSize;
                    var lines           = TextHelper.GenerateLines(canvasSize * 0.25f, font, handler.Spell.DisplayInfo).ToList();
                    var textWidth       = lines.Max(l => l.Size.X);
                    var textHeight      = lines.Max(l => l.Size.Y);
                    var timeLeft        = string.Format("{0}s", (int)Math.Round(TimeConverter.ToSeconds(handler.DecayTime - handler.Elapsed), 0));

                    var textPosition    = HUDInputManager.Instance.MousePosition;
                    textPosition.X      = MathHelper.Clamp(textPosition.X, 0.0f, canvasSize.X - textWidth);
                    textPosition.Y      = MathHelper.Clamp(textPosition.Y, 0.0f, canvasSize.Y - textHeight);

                    var target          = new Vector2(textWidth, textHeight);
                    var source          = new Vector2(Icon.Width, Icon.Height);
                    target.Y            += ElementOffset;

                    var scale = target / source;

                    spriteBatch.Draw(Background,
                                     textPosition - new Vector2(ElementOffset),
                                     null,
                                     null,
                                     null,
                                     0.0f,
                                     scale,
                                     Color.Wheat,
                                     SpriteEffects.None,
                                     0.0f);

                    spriteBatch.DrawString(font, header, textPosition, Color.White);

                    textPosition.Y += font.MeasureString(header).Y + ElementOffset;

                    foreach (var line in lines)
                    {
                        spriteBatch.DrawString(font, line.Contents, line.Position + textPosition, Color.White);
                    }

                    textPosition.Y += ElementOffset;

                    spriteBatch.DrawString(font, timeLeft, new Vector2(textPosition.X, textHeight + textPosition.Y + font.MeasureString(timeLeft).Y), Color.White);
                }
            }
        }
        private sealed class ItemIconHandler : IconElementHandler
        {
            public ItemIconHandler()
                : base()
            {
            }

            public override void Show(SpriteFont font, SpriteBatch spriteBatch, Vector2 position, Vector2 size, MouseHoverState hoverState)
            {
            }
        }
        private sealed class SpellIconHandler : IconElementHandler
        {
            public SpellIconHandler()
                : base()
            {
            }

            public override void Show(SpriteFont font, SpriteBatch spriteBatch, Vector2 position, Vector2 size, MouseHoverState hoverState)
            {
            }
        }
        #endregion

        #region Fields
        private IconElementHandler handler;
        private SpriteFont font;

        private Vector2 position;
        private Vector2 size;

        private MouseHoverState hoverState;

        private object content;
        #endregion

        public IconElement()
        {
        }
        
        public void Invalidate(Control control)
        {
            Debug.Assert(content != null);

            handler  = null;
            position = control.DisplayPosition;
            size     = control.DisplaySize;

            if (!control.ReadProperty("Font", ref font))             return;
            if (!control.ReadProperty("Content", ref content))       return;
            if (!control.ReadProperty("HoverState", ref hoverState)) return;

            if (content == null) return;

            var buff = content as SelfBuffSpellHandler;
            
            if (buff != null) { handler = new BuffIconHandler(buff); return; }
        }

        public void Show(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (handler == null) return;

            handler.Show(font, spriteBatch, position, size, hoverState);
        }
    }
}
