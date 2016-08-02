using Microsoft.Xna.Framework.Input;
using OpenTK.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vRPGEngine.Input
{
    public sealed class KeyboardBinding
    {
        #region Properties
        public const float TriggerDelta = 100.0f / 60.0f;
        #endregion

        #region Properties
        public string Name
        {
            get;
            private set;
        }

        public KeyTrigger Trigger
        {
            get;
            private set;
        }
        public Keys Keys
        {
            get;
            private set;
        }

        public KeyState LastState
        {
            get;
            set;
        }

        public Action Callback
        {
            get;
            private set;
        }
        #endregion

        public KeyboardBinding(string name, Keys keys, KeyTrigger trigger, Action callback)
        {
            Name        = name;
            Keys        = keys;
            Trigger     = trigger;
            Callback    = callback;
            LastState   = KeyState.Up;
        }
    }
}
