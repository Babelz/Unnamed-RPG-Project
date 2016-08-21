using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using vRPGEngine.Input;

namespace vRPGEngine.HUD
{
    public sealed class HUDInputManager : SystemManager<HUDInputManager>
    {
        #region Fields
        private MouseState ms;
        #endregion

        #region Properties
        public IEnumerable<MouseButton> PressedMouseButtons
        {
            get
            {
                if (ms.LeftButton == ButtonState.Pressed)     yield return MouseButton.LeftButton;
                if (ms.RightButton == ButtonState.Pressed)    yield return MouseButton.RightButton;
                if (ms.MiddleButton == ButtonState.Pressed)   yield return MouseButton.MiddleButton;
                if (ms.XButton1 == ButtonState.Pressed)       yield return MouseButton.XButton1;
                if (ms.XButton2 == ButtonState.Pressed)       yield return MouseButton.XButton2;
            }
        }
        public Vector2 MousePosition
        {
            get
            {
                return new Vector2(ms.Position.X, ms.Position.Y);
            }
        }
        public int MouseScrollValue
        {
            get
            {
                return ms.ScrollWheelValue;
            }
        }
        #endregion

        private HUDInputManager()
            : base()
        {
        }

        protected override void OnUpdate(GameTime gameTime)
        {
            ms = Mouse.GetState();
        }

        public bool MouseIntersects(Rectf displayBounds)
        {
            Rectf mouseBounds;

            mouseBounds.Position    = MousePosition;
            mouseBounds.Size        = new Vector2(1.0f);

            return displayBounds.Intersects(mouseBounds);
        }
    }
}
