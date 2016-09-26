using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using vRPGEngine.HUD.Controls;
using vRPGEngine.Core;

namespace vRPGEngine.HUD.Elements
{
    public sealed class ImageElement : IDisplayElement
    {
        #region Fields
        private Vector2 position;
        private Vector2 scale;
        private Vector2 origin;

        private Rectangle source;

        private Color color;
        private Texture2D texture;

        private SpriteEffects effects;

        private float rotation;
        #endregion

        public ImageElement()
        {
        }

        public void Invalidate(Control control)
        {
            position = control.DisplayPosition;

            if (!control.ReadProperty("Texture", ref texture)) texture = DefaultValues.MissingTexture;
            
            control.ReadProperty("Scale", ref scale);
            control.ReadProperty("Origin", ref origin);
            control.ReadProperty("Source", ref source);
            control.ReadProperty("Color", ref color);
            control.ReadProperty("Rotation", ref rotation);

            var flags = ImagePresentationFlags.None;

            control.ReadProperty("PresentationFlags", ref flags);

            if (flags.HasFlag(ImagePresentationFlags.FlipHorizontal))   effects |= SpriteEffects.FlipHorizontally;
            if (flags.HasFlag(ImagePresentationFlags.FlipVertical))     effects |= SpriteEffects.FlipVertically;
        }

        public void Show(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture,
                             position,
                             null,
                             source,
                             origin,
                             rotation,
                             scale,
                             color,
                             effects,
                             0.0f);
        }
    }
}
