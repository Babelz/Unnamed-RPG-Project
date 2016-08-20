using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vRPGEngine.HUD.Interfaces;

namespace vRPGEngine.HUD.Controls
{
    public abstract class MouseControlBase : Control, IMouseControl
    {
        #region Fields
        private MouseHoverState hoverState;
        #endregion

        #region Events
        public event MouseControlEventHandler OnMouseEnter;
        public event MouseControlEventHandler OnMouseLeave;
        public event MouseControlEventHandler OnMouseHover;
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
        #endregion

        public MouseControlBase()
            : base()
        {
            HoverState = MouseHoverState.Enter;

            RegisterProperty("HoverState", () => HoverState, (o) => HoverState = (MouseHoverState)o);
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
                    else HoverState = MouseHoverState.Leave;
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
            // Update hover.
            UpdateHoverState();
        }
    }
}
