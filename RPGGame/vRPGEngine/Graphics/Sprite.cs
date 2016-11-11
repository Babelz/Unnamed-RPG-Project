using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;
using vRPGEngine.Core;

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

                if (texture != null && Source.IsEmpty) Source = new Rectangle(0, 0, texture.Width, texture.Height);
            }
        }
        public override Vector2 Size
        {
            get
            {
                Vector2 size;

                size.X = Scale.X * texture.Width;
                size.Y = Scale.Y * texture.Height;

                return size;
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
        public SpriteEffects Effects
        {
            get;
            set;
        }
        #endregion

        public Sprite(Texture2D texture = null)
        {
            Texture     = texture;
            Source      = new Rectangle(0, 0, Texture.Width, Texture.Height);
            Position    = Vector2.Zero;
            Color       = Color.White;
            Rotation    = 0.0f;
            Effects     = SpriteEffects.None;
            Depth       = 1.0f; 
        }

        public override void ClearState()
        {
            Source      = new Rectangle(0, 0, texture?.Width ?? 0, texture?.Height ?? 0);
            Color       = Color.White;
            Rotation    = 0.0f;
            Effects     = SpriteEffects.None;

            base.ClearState();
        }

        public void CenterOrigin()
        {
            Origin = new Vector2(texture.Width / 2.0f, texture.Height / 2.0f);
        }
        public void SourceFill()
        {
            Source = new Rectangle(0, 0, texture.Width, texture.Height);
        }

        public void ScaleTo(Vector2 to)
        {
            ScaleTo(to, texture);
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
            
            spriteBatch.Draw(Texture, Position, Source, Color, Rotation, Origin, Scale, Effects, Depth);
        }
    }
}
