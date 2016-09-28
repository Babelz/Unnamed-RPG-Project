using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace vRPGEngine.Core
{
    public struct Rectf : IEnumerable<Vector2>
    {
        #region Fields
        public Vector2 Position;
        public Vector2 Size;
        #endregion

        #region Properties
        public Vector2 TopLeft
        {
            get
            {
                return Position;
            }
        }
        public Vector2 TopRight
        {
            get
            {
                return new Vector2(Position.X + Size.X, Position.Y);
            }
        }
        public Vector2 BottomLeft
        {
            get
            {
                return new Vector2(Position.X, Position.Y + Size.Y);
            }
        }
        public Vector2 BottomRight
        {
            get
            {
                return Position + Size;
            }
        }
        
        public float Left
        {
            get
            {
                return Position.X;
            }
        }
        public float Right
        {
            get
            {
                return Position.X + Size.X;
            }
        }
        public float Top
        {
            get
            {
                return Position.Y;
            }
        }
        public float Bottom
        {
            get
            {
                return Position.Y + Size.Y;
            }
        }

        public float X
        {
            get
            {
                return Position.X;
            }
        }
        public float Y
        {
            get
            {
                return Position.Y;
            }
        }
        public float Width
        {
            get
            {
                return Size.X;
            }
        }
        public float Height
        {
            get
            {
                return Size.Y;
            }
        }
        #endregion

        public Rectf(Vector2 position, Vector2 size)
        {
            Position    = position;
            Size        = size;
        }
        public Rectf(float x, float y, float width, float height)
            : this(new Vector2(x, y), new Vector2(width, height))
        {
        }
        public Rectf(float x, float y, float bounds)
            : this(new Vector2(x, y), new Vector2(bounds))
        {
        }
        public Rectf(float xy, float bounds)
            : this(new Vector2(xy), new Vector2(bounds))
        {
        }
        
        public bool Intersects(Rectf other)
        {
            return Left < other.Right &&
                   other.Left < Right &&
                   Top < other.Bottom &&
                   other.Top < Bottom;
        }

        public IEnumerator<Vector2> GetEnumerator()
        {
            yield return TopLeft;
            yield return TopRight;
            yield return BottomLeft;
            yield return BottomRight;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
