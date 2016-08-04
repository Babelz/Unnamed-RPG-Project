using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace vRPGEngine.Graphics
{
    public abstract class Renderable : IRenderable
    {
        #region Fields
        private Vector2 scale;
        private Vector2 position;
        private Vector2 size;
        #endregion

        #region Properties
        public Point Cell
        {
            get;
            set;
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

        public Vector2 Center
        {
            get
            {
                return Size / 2.0f;
            }
        }
        public Vector2 Position
        {
            get
            {
                return position;
            }
            set
            {
                position = value;

                Renderer.Instance.Invalidate(this);
            }
        }
        public Vector2 Scale
        {
            get
            {
                return scale;
            }
            set
            {
                scale = value;

                Renderer.Instance.Invalidate(this);
            }
        }
        public Vector2 Size
        {
            get
            {
                return size * scale;
            }
            set
            {
                size = value;

                Renderer.Instance.Invalidate(this);
            }
        }
        public Vector2 Origin
        {
            get;
            set;
        }

        public bool Visible
        {
            get;
            set;
        }
        public bool Active
        {
            get;
            set;
        }
        public float Depth
        {
            get;
            set;
        }
        #endregion

        public Renderable()
        {
            Scale       = new Vector2(1.0f);
            Origin      = Vector2.Zero;
            Visible     = true;
        }

        public abstract void Present(SpriteBatch spriteBatch, GameTime gameTime);
    }
}
