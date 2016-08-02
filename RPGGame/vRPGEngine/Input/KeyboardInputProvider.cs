﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;

namespace vRPGEngine.Input
{
    public interface IBindingCollection
    {
        IEnumerable<KeyboardBinding> Bindings();
    }

    public sealed class KeyboardInputProvider : IInputProvider
    {
        #region Fields
        private readonly List<KeyboardBinding> bindings;
        #endregion

        public KeyboardInputProvider()
        {
            bindings = new List<KeyboardBinding>();
        }
        
        private void UpdateUpTrigger(GameTime gameTime, KeyboardBinding binding, KeyboardState state)
        {
            var keys = state.GetPressedKeys().ToArray();

            if (!keys.Contains(binding.Keys)) binding.Callback();
        }
        private void UpdateReleasedTrigger(GameTime gameTime, KeyboardBinding binding, KeyboardState state)
        {
            var keys = state.GetPressedKeys().ToArray();

            var down = keys.Contains(binding.Keys);

            if (!down && binding.LastState == KeyState.Down) binding.Callback();

            binding.LastState = down ? KeyState.Down : KeyState.Up;
        }
        private void UpdatePressedTrigger(GameTime gameTime, KeyboardBinding binding, KeyboardState state)
        {
            var keys = state.GetPressedKeys().ToArray();

            var down = keys.Contains(binding.Keys);

            if (down && binding.LastState == KeyState.Up) binding.Callback();
            
            binding.LastState = down ? KeyState.Down : KeyState.Up;
        }
        private void UpdateDownTrigger(GameTime gameTime, KeyboardBinding binding, KeyboardState state)
        {
            var keys = state.GetPressedKeys().ToArray();

            if (keys.Contains(binding.Keys)) binding.Callback();
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

        public void Bind(string name, Keys keys, KeyTrigger trigger, Action callback)
        {
            Debug.Assert(!string.IsNullOrEmpty(name));
            Debug.Assert(callback != null);

            if (bindings.FirstOrDefault(b => b.Name == name) != null) throw new vRPGEngineException("key binding with name " + name + " already exists");

            bindings.Add(new KeyboardBinding(name, keys, trigger, callback));
        }
        public bool Unbind(string name)
        {
            Debug.Assert(!string.IsNullOrEmpty(name));

            return bindings.Remove(bindings.Find(b => b.Name == name));
        }

        public void Bind(IBindingCollection bindings)
        {
            Debug.Assert(bindings != null);

            foreach (var binding in bindings.Bindings())
            {
                if (binding == null)                                                    throw new vRPGEngineException("bindings can't be null!");
                if (this.bindings.FirstOrDefault(b => b.Name == binding.Name) != null)  throw new vRPGEngineException("key binding with name " + binding.Name + " already exists");

                this.bindings.Add(binding);
            }
        }
        public int Unbind(IBindingCollection bindings)
        {
            Debug.Assert(bindings != null);

            var count = 0;

            foreach (var binding in bindings.Bindings())
            {
                if (binding == null) throw new vRPGEngineException("bindings can't be null!");

                if (this.bindings.Remove(binding)) count++;
            }

            return count;
        }
    }
}
