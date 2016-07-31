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
        public string Name
        {
            get;
            set;
        }
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

        public View(Viewport viewport)
        {
            Viewport    = viewport;
            Name        = string.Empty;
            Position    = Vector2.Zero;
            Origin      = new Vector2(viewport.Width * 0.5f, viewport.Height * 0.5f);
            Zoom        = 1.0f;
            Rotation    = 0.0f;
        }

        public void FocuCenter()
        {
            Origin = Vector2.Zero;
        }
        public void FocusTopLeft()
        {
            Origin = new Vector2(Viewport.Width * 0.5f, Viewport.Height * 0.5f);
        }
        public void FocusBottomRight()
        {
            Origin = new Vector2(Viewport.Width, Viewport.Height);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Matrix Transform()
        {
            return Matrix.CreateTranslation(new Vector3(-(Position.X + Origin.X), -(Position.Y + Origin.Y), 0.0f)) *
                   Matrix.CreateRotationZ(Rotation) *
                   Matrix.CreateScale(new Vector3(Zoom, Zoom, 1.0f)) *
                   Matrix.CreateTranslation(new Vector3(Viewport.Width * 0.5f, Viewport.Height * 0.5f, 0.0f));
        }
    }
}
