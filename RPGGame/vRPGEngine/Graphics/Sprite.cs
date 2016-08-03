using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;

namespace vRPGEngine.Graphics
{
    public sealed class Sprite : Renderable
    {
        #region Fields
        private Texture2D texture;
        #endregion

        #region Properties
        public Texture2D Texture
        {
            get
            {
                return texture;
            }
            set
            {
                texture = value;

                if (texture == null) texture = DefaultValues.MissingTexture;
            }
        }
        public Rectangle Source
        {
            get;
            set;
        }
        public Color Color
        {
            get;
            set;
        }
        public float Rotation
        {
            get;
            set;
        }
        #endregion

        public Sprite(Texture2D texture)
        {
            Texture     = texture;
            Source      = new Rectangle(0, 0, Texture.Width, Texture.Height);
            Position    = Vector2.Zero;
            Color       = Color.White;
            Rotation    = 0.0f;
        }

        public override void Present(SpriteBatch spriteBatch, GameTime gameTime)
        {
            //spriteBatch.Draw(Texture,
            //                 Position,
            //                 null,
            //                 Source,
            //                 Vector2.Zero,
            //                 Rotation,
            //                 Scale,
            //                 Color,
            //                 SpriteEffects.None,
            //                 0.0f);

            spriteBatch.Draw(Texture, Position, Source, Color, Rotation, Vector2.Zero, Scale, SpriteEffects.None, 0.0f);
        }
    }
}
