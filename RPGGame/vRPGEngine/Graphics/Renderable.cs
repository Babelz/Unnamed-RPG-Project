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

        private int layer;
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
            get
            {
                return layer;
            }
            set
            {
                if (layer == value) return;
                
                Renderer.Instance.Remove(this);

                layer = value;

                Renderer.Instance.Add(this, layer);
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

        public Vector2 Center
        {
            get
            {
                return Size / 2;
            }
        }

        public abstract Vector2 Size
        {
            get;
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

        public virtual void ClearState()
        {
            Cell     = Point.Zero;
            Index    = 0;
            Layer    = 0;

            Position = Vector2.Zero;
            Scale    = new Vector2(1.0f);

            Origin   = Vector2.Zero;
            Visible  = true;
            Active   = false;
            Depth    = 0.0f;
        }

        public void ScaleTo(Vector2 to, Vector2 from)
        {
            var min = Vector2.Min(to, from);
            var max = Vector2.Max(to, from);

            Scale = min / max;
        }

        public void ScaleTo(Vector2 to, Texture2D from)
        {
            Vector2 texVec;

            texVec.X = from.Width;
            texVec.Y = from.Height;

            ScaleTo(to, texVec);
        }

        public abstract void Present(SpriteBatch spriteBatch, GameTime gameTime);
    }
}
