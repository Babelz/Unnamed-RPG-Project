using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vRPGEngine.Graphics;

namespace vRPGEngine.ECS.Components
{
    public sealed class Camera : Component<Camera>
    {
        #region Fields
        private View view;
        #endregion

        #region Properties
        public string Name
        {
            get;
            set;
        }
        public Vector2 Position
        {
            get
            {
                return view.Position;
            }
            set
            {
                view.Position = value;
            }
        }
        public Vector2 Origin
        {
            get
            {
                return view.Origin;
            }
            set
            {
                view.Origin = value;
            }
        }
        public float Zoom
        {
            get
            {
                return view.Zoom;
            }
            set
            {
                view.Zoom = value;
            }
        }
        public float Rotation
        {
            get
            {
                return view.Rotation;
            }
            set
            {
                view.Rotation = value;
            }
        }
        public Viewport Viewport
        {
            get
            {
                return view.Viewport;
            }
            set
            {
                view.Viewport = value;
            }
        }
        public Matrix Transform
        {
            get
            {
                return view.Transform;
            }
        }

        public Vector2 VisibleArea
        {
            get
            {
                return view.VisibleArea;
            }
        }

        public View View
        {
            get
            {
                return view;
            }
        }
        #endregion

        public Camera()
            : base()
        {
            view = new View(Engine.Instance.GraphicsDevice.Viewport);
        }

        protected override void Initialize()
        {
            Renderer.Instance.RegisterView(view);
        }
        protected override void Deinitialize()
        {
            Renderer.Instance.UnregisterView(view);
        }

        public void FocuCenter()
        {
            view.FocuCenter();
        }
        public void FocusTopLeft()
        {
            view.FocusTopLeft();
        }
        public void FocusBottomRight()
        {
            view.FocusBottomRight();
        }

        public void ComputeTransform()
        {
            view.ComputeTransform();
        }

        public Vector2 ScreenToCamera(Vector2 screenPosition)
        {
            return view.ScreenToView(screenPosition);
        }
        public Vector2 ScreenToCamera(Point screenPosition)
        {
            return ScreenToCamera(new Vector2(screenPosition.X, screenPosition.Y));
        }
    }
}
