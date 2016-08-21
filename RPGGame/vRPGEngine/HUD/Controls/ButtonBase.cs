using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using vRPGEngine.HUD.Interfaces;
using vRPGEngine.Input;
using Microsoft.Xna.Framework.Input;

namespace vRPGEngine.HUD.Controls
{
    public enum MouseHoverState
    {
        Enter,
        Hover,
        Leave
    }

    public enum ButtonControlState
    {
        Up,
        Down,
        Pressed,
        Released,
    }

    public abstract class ButtonBase : MouseControlBase, IButtonControl
    {
        #region Fields
        private ButtonControlState buttonState;

        private MouseButton trigger;
        #endregion

        #region Events
        public event ButtonMouseEventHandler ButtonDown;
        public event ButtonMouseEventHandler ButtonUp;

        public event ButtonMouseEventHandler ButtonPressed;
        public event ButtonMouseEventHandler ButtonReleased;

        // TODO: impl keyboard.
        #endregion

        #region Properties
        protected ButtonControlState ButtonState
        {
            get
            {
                return buttonState;
            }
            set
            {
                buttonState = value;

                NotifyPropertyChanged("HoverState");
            }
        }

        public MouseButton Trigger
        {
            get
            {
                return trigger;
            }
            set
            {
                trigger = value;

                NotifyPropertyChanged("Trigger");
            }
        }
        #endregion

        public ButtonBase()
            : base()
        {
            Trigger     = MouseButton.LeftButton;
            ButtonState = ButtonControlState.Up;

            RegisterProperty("ButtonState", () => ButtonState, (o) => ButtonState = (ButtonControlState)o);
            RegisterProperty("Trigger", () => Trigger, (o) => Trigger = (MouseButton)o);
        }

        private void UpdateButtonState()
        {
            var isTriggerDown = HUDInputManager.Instance.PressedMouseButtons.Contains(Trigger);
            var intersects    = HUDInputManager.Instance.MouseIntersects(DisplayBounds);

            switch (ButtonState)
            {
                case ButtonControlState.Up:
                    if      (!isTriggerDown)  ButtonUp?.Invoke(this);
                    else if (intersects)      ButtonState = ButtonControlState.Pressed;
                    break;
                case ButtonControlState.Down:
                    if (isTriggerDown) ButtonDown?.Invoke(this);
                    else               ButtonState = ButtonControlState.Released;
                    break;
                case ButtonControlState.Pressed:
                    if (isTriggerDown)
                    {
                        ButtonPressed?.Invoke(this);

                        ButtonState = ButtonControlState.Down;
                    }
                    else
                    {
                        ButtonState = ButtonControlState.Released;
                    }
                    break;
                case ButtonControlState.Released:
                    if (intersects) ButtonReleased?.Invoke(this);

                    ButtonState = ButtonControlState.Up;
                    break;
                default:
                    break;
            }
        }

        protected override void OnUpdate(GameTime gameTime)
        {
            base.OnUpdate(gameTime);

            // Update button state.
            UpdateButtonState();
        }
    }
}
