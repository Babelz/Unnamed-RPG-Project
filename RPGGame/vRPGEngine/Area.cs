using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace vRPGEngine
{
    public struct Area : IEnumerable<Vector2>
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
        #endregion

        public Area(Vector2 position, Vector2 size)
        {
            Position    = position;
            Size        = size;
        }

        public IEnumerator<Vector2> GetEnumerator()
        {
            var points = new List<Vector2>
            {
                TopLeft,
                TopRight,
                BottomLeft,
                BottomRight
            };

            return points.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
