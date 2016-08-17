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

    public abstract class ButtonBase : Control, IButtonControl
    {
        #region Fields
        private ButtonControlState buttonState;
        private MouseHoverState hoverState;

        private MouseButton trigger;
        #endregion

        #region Events
        public event ButtonMouseEventHandler OnMouseEnter;
        public event ButtonMouseEventHandler OnMouseLeave;
        public event ButtonMouseEventHandler OnMouseHover;

        public event ButtonMouseEventHandler ButtonDown;
        public event ButtonMouseEventHandler ButtonUp;

        public event ButtonMouseEventHandler ButtonPressed;
        public event ButtonMouseEventHandler ButtonReleased;

        // TODO: impl keyboard.
        #endregion

        #region Properties
        protected MouseHoverState HoverState
        {
            get
            {
                return hoverState;
            }
            set
            {
                hoverState = value;

                NotifyPropertyChanged("HoverState");
            }
        }
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
            HoverState  = MouseHoverState.Enter;

            RegisterProperty("HoverState", () => HoverState, (o) => HoverState = (MouseHoverState)o);
            RegisterProperty("ButtonState", () => ButtonState, (o) => ButtonState = (ButtonControlState)o);
            RegisterProperty("Trigger", () => Trigger, (o) => Trigger = (MouseButton)o);
        }

        private void UpdateButtonState()
        {
            var isTriggerDown = HUDInputManager.Instance.PressedButtons.Contains(Trigger);
            var intersects    = HUDInputManager.Instance.Intersects(DisplayBounds);

            switch (ButtonState)
            {
                case ButtonControlState.Up:
                    if (!isTriggerDown) ButtonUp?.Invoke(this);
                    else                ButtonState = ButtonControlState.Pressed;
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

        private void UpdateHoverState()
        {
            var intersects = HUDInputManager.Instance.Intersects(DisplayBounds);

            switch (HoverState)
            {
                case MouseHoverState.Enter:
                    if (intersects)
                    {
                        OnMouseEnter?.Invoke(this);

                        HoverState = MouseHoverState.Hover;
                    }
                    break;
                case MouseHoverState.Hover:
                    if (intersects) OnMouseHover?.Invoke(this);
                    else            HoverState = MouseHoverState.Leave;
                    break;
                case MouseHoverState.Leave:
                    if (!intersects)
                    {
                        OnMouseLeave?.Invoke(this);

                        HoverState = MouseHoverState.Enter;
                    }
                    else
                    {
                        HoverState = MouseHoverState.Hover;
                    }
                    break;
                default:
                    break;
            }
        }

        protected override void OnUpdate(GameTime gameTime)
        {
            // Update button state.
            UpdateButtonState();

            // Update hover.
            UpdateHoverState();
        }
    }
}
