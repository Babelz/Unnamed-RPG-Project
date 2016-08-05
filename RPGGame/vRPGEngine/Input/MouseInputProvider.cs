using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using System.Diagnostics;
using Microsoft.Xna.Framework.Input;
using OpenTK.Input;

using Mouse         = Microsoft.Xna.Framework.Input.Mouse;
using MouseState    = Microsoft.Xna.Framework.Input.MouseState;

namespace vRPGEngine.Input
{
    public delegate void MouseInputEventHandler(MouseState state);

    public sealed class MouseInputProvider : IInputProvider
    {
        #region Fields
        private readonly List<MouseBinding> bindings;

        private Point lastPosition;
        private float lastScrollValue;
        #endregion

        #region Events
        public event MouseInputEventHandler Move;
        public event MouseInputEventHandler Scroll;
        #endregion

        public MouseInputProvider()
        {
            bindings = new List<MouseBinding>();
        }
        
        private IEnumerable<MouseButton> GetPressedButtons(MouseState state)
        {
            var buttons = new List<MouseButton>();

            if (state.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed)      buttons.Add(MouseButton.LeftButton);
            if (state.RightButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed)     buttons.Add(MouseButton.RightButton);
            if (state.MiddleButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed)    buttons.Add(MouseButton.MiddleButton);
            if (state.XButton1 == Microsoft.Xna.Framework.Input.ButtonState.Pressed)        buttons.Add(MouseButton.XButton1);
            if (state.XButton2 == Microsoft.Xna.Framework.Input.ButtonState.Pressed)        buttons.Add(MouseButton.XButton2);

            return buttons;
        }

        private void UpdateEvents(MouseState state, Point currentPosition, float currentScrollValue)
        {
            if (currentPosition != lastPosition)
            {
                lastPosition = currentPosition;

                Move?.Invoke(state);
            }

            if (currentScrollValue != lastScrollValue)
            {
                lastScrollValue = currentScrollValue;

                Scroll?.Invoke(state);
            }
        }
       
        private void UpdateDownTrigger(MouseBinding binding, MouseState state)
        {
            var buttons = GetPressedButtons(state);

            if (buttons.Contains(binding.Buttons)) binding.Callback(state);
        }
        private void UpdatePressedTrigger(MouseBinding binding, MouseState state)
        {
            var buttons = GetPressedButtons(state);

            var down = buttons.Contains(binding.Buttons);

            if (down && binding.LastState == OpenTK.Input.ButtonState.Released) binding.Callback(state);

            binding.LastState = down ? OpenTK.Input.ButtonState.Pressed : OpenTK.Input.ButtonState.Released;
        }
        private void UpdateReleasedTrigger(MouseBinding binding, MouseState state)
        {
            var buttons = GetPressedButtons(state);

            var down = buttons.Contains(binding.Buttons);

            if (!down && binding.LastState == OpenTK.Input.ButtonState.Pressed) binding.Callback(state);

            binding.LastState = down ? OpenTK.Input.ButtonState.Pressed : OpenTK.Input.ButtonState.Released;
        }
        private void UpdateUpTrigger(MouseBinding binding, MouseState state)
        {
            var buttons = GetPressedButtons(state);

            if (!buttons.Contains(binding.Buttons)) binding.Callback(state);
        }

        public void Update(GameTime gameTime)
        {
            var state = Mouse.GetState();

            var currentPosition     = new Point(state.X, state.Y);
            var currentScrollValue  = state.ScrollWheelValue;

            UpdateEvents(state, currentPosition, currentScrollValue);

            foreach (var binding in bindings)
            {
                switch (binding.Trigger)
                {
                    case MouseTrigger.Up:      UpdateUpTrigger(binding, state);        break;
                    case MouseTrigger.Relesed: UpdateReleasedTrigger(binding, state);  break;
                    case MouseTrigger.Pressed: UpdatePressedTrigger(binding, state);   break;
                    case MouseTrigger.Down:    UpdateDownTrigger(binding, state);      break;
                }
            }
        }

        public void Bind(string name, MouseButton buttons, MouseTrigger trigger, Action<MouseState> callback)
        {
            Debug.Assert(!string.IsNullOrEmpty(name));
            Debug.Assert(callback != null);

            if (bindings.FirstOrDefault(b => b.Name == name) != null) throw new vRPGEngineException("mouse binding with name " + name + " already exists");

            bindings.Add(new MouseBinding(name, trigger, buttons, callback));
        }
        public bool Unbind(string name)
        {
            Debug.Assert(!string.IsNullOrEmpty(name));

            return bindings.Remove(bindings.Find(b => b.Name == name));
        }

        public void Bind(IMouseBindingCollection bindings)
        {
            Debug.Assert(bindings != null);

            foreach (var binding in bindings.Bindings())
            {
                if (binding == null)                                                    throw new vRPGEngineException("bindings can't be null!");
                if (this.bindings.FirstOrDefault(b => b.Name == binding.Name) != null)  throw new vRPGEngineException("key binding with name " + binding.Name + " already exists");

                this.bindings.Add(binding);
            }

            foreach (var scroll in bindings.ScrollBindings())   Scroll += scroll;
            foreach (var move in bindings.MoveBindings())       Move += move;
        }
        public int Unbind(IMouseBindingCollection bindings)
        {
            Debug.Assert(bindings != null);

            var count = 0;

            foreach (var binding in bindings.Bindings())
            {
                if (binding == null) throw new vRPGEngineException("bindings can't be null!");

                if (this.bindings.Remove(binding)) count++;
            }
            
            foreach (var scroll in bindings.ScrollBindings())   Scroll -= scroll;
            foreach (var move in bindings.MoveBindings())       Move -= move;

            return count;
        }
    }
}
