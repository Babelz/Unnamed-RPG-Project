using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace vRPGEngine.Graphics
{
    public sealed class View
    {
        #region Properties
        public Vector2 Position
        {
            get;
            set;
        }
        public Vector2 Origin
        {
            get;
            set;
        }
        public float Zoom
        {
            get;
            set;
        }
        public float Rotation
        {
            get;
            set;
        }
        public Viewport Viewport
        {
            get;
            set;
        }
        #endregion

        public View()
        {
            Position    = Vector2.Zero;
            Origin      = Vector2.Zero;
            Zoom        = 1.0f;
            Rotation    = 0.0f;
        }

        public void Center()
        {
            Origin = new Vector2(Viewport.Width * 0.5f, Viewport.Height * 0.5f);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Matrix Transform()
        {
            return Matrix.CreateTranslation(new Vector3(-Position.X, -Position.Y, 0.0f)) *
                   Matrix.CreateRotationZ(Rotation) *
                   Matrix.CreateScale(new Vector3(Zoom, Zoom, 1.0f)) *
                   Matrix.CreateTranslation(new Vector3(Viewport.Width - Origin.X, Viewport.Height - Origin.Y, 0.0f));
        }
    }
}
