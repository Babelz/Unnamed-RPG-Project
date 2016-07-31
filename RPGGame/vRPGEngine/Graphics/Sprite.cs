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
    public sealed class Sprite : IRenderable
    {
        #region Properties
        public Point Cell
        {
            get;
            set;
        }
        public Vector2 Center
        {
            get
            {
                return Position + Size / 2.0f;
            }
        }
        public int Index
        {
            get;
            set;
        }
        public int Layer
        {
            get;
            set;
        }
        public Vector2 Position
        {
            get;
            set;
        }
        public Vector2 Scale
        {
            get;
            set;
        }
        public Vector2 Size
        {
            get
            {
                return new Vector2(Texture.Width * Scale.X, Texture.Height * Scale.Y);
            }
        }
        public bool Visible
        {
            get;
            set;
        }
        public Texture2D Texture
        {
            get;
            set;
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
            Debug.Assert(texture != null);

            Texture     = texture;
            Source      = new Rectangle(0, 0, texture.Width, texture.Height);
            Scale       = new Vector2(1.0f);
            Position    = Vector2.Zero;
            Color       = Color.White;
            Visible     = true;
            Rotation    = 0.0f;
        }

        public void Present(SpriteBatch spriteBatch)
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
