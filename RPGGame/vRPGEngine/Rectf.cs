using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace vRPGEngine
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
        public float W
        {
            get
            {
                return Size.X;
            }
        }
        public float H
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

        public static Rectf Empty;

        public bool Intersects(Rectf other)
        {
            return Left < other.Right &&
                   Left < other.Right &&
                   Top < other.Bottom &&
                   Top < other.Bottom;
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
