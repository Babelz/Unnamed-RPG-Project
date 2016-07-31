using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace vRPGEngine.Input
{
    public sealed class KeyboardInputProvider : IInputProvider
    {
        #region Fields
        private readonly List<KeyboardBinding> bindings;
        #endregion

        public KeyboardInputProvider()
        {
        }
        
        private void UpdateUpTrigger(GameTime gameTime, KeyboardBinding binding, KeyboardState state)
        {
        }
        private void UpdateReleasedTrigger(GameTime gameTime, KeyboardBinding binding, KeyboardState state)
        {
        }
        private void UpdatePressedTrigger(GameTime gameTime, KeyboardBinding binding, KeyboardState state)
        {
        }
        private void UpdateDownTrigger(GameTime gameTime, KeyboardBinding binding, KeyboardState state)
        {
        }

        public void Update(GameTime gameTime)
        {
            if (bindings.Count == 0) return;

            var state = Keyboard.GetState();

            foreach (var binding in bindings)
            {
                switch (binding.Trigger)
                {
                    case KeyTrigger.Up:         UpdateUpTrigger(gameTime, binding, state);                 break;
                    case KeyTrigger.Released:   UpdateReleasedTrigger(gameTime, binding, state);           break;
                    case KeyTrigger.Pressed:    UpdatePressedTrigger(gameTime, binding, state);            break;
                    case KeyTrigger.Down:       UpdateDownTrigger(gameTime, binding, state);               break;
                }
            }
        }
    }
}
