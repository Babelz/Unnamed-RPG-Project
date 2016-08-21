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

            public abstract void Show(SpriteFont font, SpriteBatch spriteBatch, Vector2 position, Vector2 size, MouseHoverState hoverState, object content);
        }

        private sealed class BuffIconHandler : IconElementHandler
        {
            #region Fields
            private readonly SelfBuffSpellHandler handler;
            #endregion

            public BuffIconHandler(SelfBuffSpellHandler handler)
                : base()
            {
                Debug.Assert(handler != null);

                this.handler = handler;

                Icon         = Engine.Instance.Content.Load<Texture2D>(handler.Spell.IconName);
            }

            public override void Show(SpriteFont font, SpriteBatch spriteBatch, Vector2 position, Vector2 size, MouseHoverState hoverState, object content)
            {
                var buff = content as Buff;
                
                Debug.Assert(buff != null);

                var iconScale   = size;
                iconScale.X     /= Icon.Width;
                iconScale.Y     /= Icon.Height;

                spriteBatch.Draw(Icon,
                                 position,
                                 null,
                                 null,
                                 null,
                                 0.0f,
                                 iconScale,
                                 Color.White,
                                 SpriteEffects.None,
                                 0.0f);

                if (hoverState == MouseHoverState.Hover)
                {
                    var header = buff.FromSpell.Name;
                    var lines = TextHelper.GenerateLines(size, font, buff.FromSpell.Description).ToList();
                    var mousePosition = HUDInputManager.Instance.MousePosition;
                    var canvasSize = HUDRenderer.Instance.CanvasSize;
                    var textWidth = lines.Max(l => l.Size.X);
                    var textHeight = lines.Max(l => l.Position.Y + l.Size.Y);
                    var scale = new Vector2(textWidth, textHeight) / new Vector2(Background.Width, Background.Height);
                    var timeLeft = (int)Math.Round(TimeConverter.ToSeconds(handler.DecayTime - handler.Elapsed), 0);

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

                    var timeLeftStr = string.Format("{0}s", timeLeft);

                    spriteBatch.DrawString(font, timeLeftStr, new Vector2(mousePosition.X, textHeight + mousePosition.Y + font.MeasureString(timeLeftStr).Y), Color.White);
                }
            }
        }
        private sealed class ItemIconHandler : IconElementHandler
        {
            public ItemIconHandler()
                : base()
            {
            }

            public override void Show(SpriteFont font, SpriteBatch spriteBatch, Vector2 position, Vector2 size, MouseHoverState hoverState, object content)
            {
            }
        }
        private sealed class SpellIconHandler : IconElementHandler
        {
            public SpellIconHandler()
                : base()
            {
            }

            public override void Show(SpriteFont font, SpriteBatch spriteBatch, Vector2 position, Vector2 size, MouseHoverState hoverState, object content)
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
            position = control.DisplaySize;
            size     = HUDRenderer.Instance.CanvasSize * 0.1f;

            if (!control.ReadProperty("Font", ref font))       return;
            if (!control.ReadProperty("Content", ref content)) return;

            if (content == null) return;

            var buff = content as SelfBuffSpellHandler;
            
            if (buff != null) { handler = new BuffIconHandler(buff); return; }
        }

        public void Show(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (handler == null) return;

            handler.Show(font, spriteBatch, position, size, hoverState, content);
        }
    }
}
