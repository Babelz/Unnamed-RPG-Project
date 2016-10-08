using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using vRPGEngine.HUD.Controls;

namespace vRPGEngine.HUD.Elements
{
    public struct PartData
    {
        public Vector2 Position;
        public Vector2 Scale;
        public Rectangle Source;
    }

    public sealed class StatusBarElement : IStatusBarElement
    {
        #region Fields
        private StatusBarTextureSources sources;
        private StatusBarBindingsSource bindings;

        private SpriteFont font;
        private Texture2D texture;

        private bool showText;

        private TextType textType;

        private Vector2 size;
        private Vector2 positon;

        private PartData left;
        private PartData middle;
        private PartData right;

        private Vector2 textPosition;
        private Vector2 textScale;

        private string text;
        #endregion

        public StatusBarElement()
        {
        }
        
        public void SetPresentationData(string texture, StatusBarTextureSources sources, StatusBarBindingsSource bindings)
        {
            this.texture  = Engine.Instance.Content.Load<Texture2D>(texture);
            this.sources  = sources;
            this.bindings = bindings;
        }

        public void Invalidate(Control control)
        {
            if (!control.ReadProperty("ShowText", ref showText)) showText = false;
            if (!control.ReadProperty("TextType", ref textType)) textType = TextType.None;

            if (!control.ReadProperty("Font", ref font)) font = Engine.Instance.Content.Load<SpriteFont>("default font");

            size        = control.DisplaySize;
            positon     = control.DisplayPosition;
            text        = string.Empty;
        }
        public void Show(GameTime gameTime, SpriteBatch spriteBatch)
        {
            var width   = sources.Left.Width + sources.Middle.Width + sources.Right.Width;
            var height  = Math.Max(sources.Left.Height, Math.Max(sources.Middle.Height, sources.Left.Height));

            // Compute left part position.
            left.Position       = positon;
            left.Source         = sources.Left;
            left.Scale          = new Vector2(size.X / 6.0f / sources.Left.Width, size.Y / sources.Left.Height);

            // Compute bar size.
            middle.Position.X   = left.Position.X + (left.Source.Width * left.Scale.X);
            middle.Position.Y   = left.Position.Y;
            middle.Source       = sources.Middle;
            middle.Scale        = new Vector2(size.X / (sources.Middle.Width + left.Scale.X * left.Source.Width), size.Y / sources.Middle.Height);

            // Compute left part position.
            right.Position.X    = middle.Position.X + middle.Source.Width * middle.Scale.X;
            right.Position.Y    = middle.Position.Y;
            right.Source        = sources.Right;
            right.Scale         = left.Scale;

            if (showText && font != null && bindings.Bound())
            {
                var value       = bindings.Value();
                var min         = bindings.Min();
                var max         = bindings.Max();

                var percentage  = (float)(Math.Round((bindings.Value() / (float)bindings.Max() * 100.0f)));

                if (float.IsNaN(percentage)) percentage = 0.0f;

                // Format text.
                switch (textType)
                {
                    case TextType.Value:        text = string.Format("{0} / {1}", value, max);                          break;
                    case TextType.Percentage:   text = string.Format("{0}%", percentage);                               break;
                    case TextType.Both:         text = string.Format("{0} / {1}%", bindings.Value(), percentage);       break;
                    case TextType.None: default:                                                                        break;
                }

                // Compute position and scale.
                var textSize    = font.MeasureString(text);
                textScale       = Vector2.One;
                textScale       -= new Vector2(0.50f);

                textPosition    = middle.Position;
                textPosition.X  += (middle.Source.Width * middle.Scale.X) / 2.0f - (textSize.X * textScale.X) / 2.0f;
                textPosition.Y  += (middle.Source.Height * middle.Scale.Y) / 2.0f - (textSize.Y * textScale.Y) / 2.0f;

                var middleStep  = middle.Scale.X / bindings.Max();
                middle.Scale.X  = middleStep * bindings.Value();
            }

            // Draw left.
            spriteBatch.Draw(texture,
                             left.Position,
                             null,
                             left.Source,
                             Vector2.Zero,
                             0.0f,
                             left.Scale,
                             Color.White,
                             SpriteEffects.None,
                             0.0f);

            // Draw middle.
            spriteBatch.Draw(texture,
                             middle.Position,
                             null,
                             middle.Source,
                             Vector2.Zero,
                             0.0f,
                             middle.Scale,
                             Color.White,
                             SpriteEffects.None,
                             0.0f);

            // Draw right.
            spriteBatch.Draw(texture,
                             right.Position,
                             null,
                             right.Source,
                             Vector2.Zero,
                             0.0f,
                             right.Scale,
                             Color.White,
                             SpriteEffects.None,
                             0.0f);

            // Draw text.
            spriteBatch.DrawString(font,
                                   text,
                                   textPosition,
                                   Color.Wheat,
                                   0.0f,
                                   Vector2.Zero,
                                   textScale,
                                   SpriteEffects.None,
                                   0.0f);
        }
    }
}
