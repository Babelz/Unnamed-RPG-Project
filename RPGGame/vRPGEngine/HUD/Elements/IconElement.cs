﻿using System;
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
    #region Icon handlers
    public abstract class IconElementHandler
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

        public abstract void SetContent(object content);

        public abstract void Show(SpriteFont font, SpriteBatch spriteBatch, Vector2 position, Vector2 size, MouseHoverState hoverState);
    }

    public abstract class SpellIconHandler<T> : IconElementHandler where T : class
    {
        #region Fields
        private readonly BackgroundTextureSplitter splitter;
        #endregion

        #region Properties
        protected T Handler
        {
            get;
            private set;
        }
        protected abstract string Header
        {
            get;
        }
        protected abstract string DisplayInfo
        {
            get;
        }
        protected abstract string AdditionalDisplayInfo
        {
            get;
        }
        #endregion

        public SpellIconHandler()
            : base()
        {
            splitter = new BackgroundTextureSplitter(3);
        }

        private void DrawInfo(SpriteFont font, SpriteBatch spriteBatch)
        {
            var canvasSize = HUDRenderer.Instance.CanvasSize;
            var area = HUDRenderer.Instance.CanvasSize * 0.25f;
            var lines = TextHelper.GenerateLines(area, font, DisplayInfo).ToList();
            var textWidth = lines.Max(l => l.Size.X);
            var textHeight = lines.Max(l => l.Size.Y + l.Position.Y) + font.MeasureString(Header).Y;

            if (!string.IsNullOrEmpty(AdditionalDisplayInfo)) textHeight += font.MeasureString(AdditionalDisplayInfo).Y;

            var textPosition = HUDInputManager.Instance.MousePosition;
            textPosition.X = MathHelper.Clamp(textPosition.X, 0.0f, canvasSize.X - textWidth);
            textPosition.Y = MathHelper.Clamp(textPosition.Y, 0.0f, canvasSize.Y - textHeight);

            spriteBatch.Draw(Background,
                             new Rectangle((int)(textPosition.X),
                                           (int)(textPosition.Y),
                                           (int)(textWidth),
                                           (int)(textHeight)),
                             Color.Wheat);

            spriteBatch.DrawString(font, Header, textPosition, Color.White);
            textPosition.Y += font.MeasureString(Header).Y;

            foreach (var line in lines)
            {
                textPosition += line.Position;

                spriteBatch.DrawString(font, line.Contents, textPosition, Color.White);
            }

            if (!string.IsNullOrEmpty(AdditionalDisplayInfo))
                spriteBatch.DrawString(font, AdditionalDisplayInfo, new Vector2(textPosition.X, textPosition.Y + font.MeasureString(AdditionalDisplayInfo).Y), Color.White);
        }
        private void DrawIcon(SpriteBatch spriteBatch, Vector2 position, Vector2 size)
        {
            var iconScale = size;
            iconScale.X /= Icon.Width;
            iconScale.Y /= Icon.Height;

            foreach (var part in splitter.Split(Icon))
            {
                var partPosition = new Vector2(part.X * iconScale.X, part.Y * iconScale.Y) + position;
                var partSize = new Vector2(part.Width * iconScale.X, part.Height * iconScale.Y);
                var partDestionation = new Rectangle((int)partPosition.X, (int)partPosition.Y, (int)partSize.X, (int)partSize.Y);

                spriteBatch.Draw(Icon, partDestionation, part, Color.White);
            }
        }

        public override void SetContent(object content)
        {
            Handler = content as T;
        }

        public override void Show(SpriteFont font, SpriteBatch spriteBatch, Vector2 position, Vector2 size, MouseHoverState hoverState)
        {
            if (Handler == null) return;

            DrawIcon(spriteBatch, position, size);

            if (hoverState == MouseHoverState.Hover) DrawInfo(font, spriteBatch);
        }
    }

    public sealed class BuffIconHandler : SpellIconHandler<SelfBuffSpellHandler>
    {
        #region Properties
        protected override string AdditionalDisplayInfo
        {
            get
            {
                return string.Format("{0}s", (int)Math.Round(TimeConverter.ToSeconds(Handler.DecayTime - Handler.Elapsed), 0));
            }
        }
        protected override string DisplayInfo
        {
            get
            {
                return Handler.Spell.DisplayInfo;
            }
        }
        protected override string Header
        {
            get
            {
                return Handler.Spell.Name;
            }
        }
        #endregion

        public override void SetContent(object content)
        {
            base.SetContent(content);

            if (Handler != null) Icon = Engine.Instance.Content.Load<Texture2D>(Handler.Spell.IconName);
        }

        public BuffIconHandler()
            : base()
        {
        }
    }
    public sealed class ItemIconHandler : IconElementHandler
    {
        public ItemIconHandler()
            : base()
        {
        }

        public override void SetContent(object content)
        {
            throw new NotImplementedException();
        }

        public override void Show(SpriteFont font, SpriteBatch spriteBatch, Vector2 position, Vector2 size, MouseHoverState hoverState)
        {
        }
    }
    public sealed class SpellIconHandler : SpellIconHandler<SpellHandler>
    {
        #region Properties
        protected override string AdditionalDisplayInfo
        {
            get
            {
                return Handler.Spell.CostDisplayString();
            }
        }
        protected override string DisplayInfo
        {
            get
            {
                return Handler.Spell.Description;
            }
        }
        protected override string Header
        {
            get
            {
                return Handler.Spell.Name;
            }
        }
        #endregion

        public SpellIconHandler()
            : base()
        {
        }

        public override void SetContent(object content)
        {
            base.SetContent(content);

            if (Handler != null) Icon = Engine.Instance.Content.Load<Texture2D>(Handler.Spell.IconName);
        }

        public override void Show(SpriteFont font, SpriteBatch spriteBatch, Vector2 position, Vector2 size, MouseHoverState hoverState)
        {
            base.Show(font, spriteBatch, position, size, hoverState);

            // TODO: show binding key.
            // TODO: show cooldown.
        }
    }
    #endregion

    public sealed class IconElement : IDisplayElement
    {
        #region Fields
        private readonly IconElementHandler handler;
        private SpriteFont font;

        private Vector2 position;
        private Vector2 size;

        private MouseHoverState hoverState;

        private object content;
        #endregion

        public IconElement(IconElementHandler handler)
        {
            Debug.Assert(handler != null);

            this.handler = handler;
        }

        public void Invalidate(Control control)
        {
            Debug.Assert(content != null);

            position = control.DisplayPosition;
            size     = control.DisplaySize;

            if (!control.ReadProperty("Font", ref font))             return;
            if (!control.ReadProperty("Content", ref content))       return;
            if (!control.ReadProperty("HoverState", ref hoverState)) return;

            handler.SetContent(content);
        }

        public void Show(GameTime gameTime, SpriteBatch spriteBatch)
        {
            handler.Show(font, spriteBatch, position, size, hoverState);
        }
    }
}
