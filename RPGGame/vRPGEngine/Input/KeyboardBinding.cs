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
        #region Constants
        public const float TimeFromReleasedToPressed = 25.0f;
        #endregion

        #region Properties
        public string Name
        {
            get;
            private set;
        }
        public float Elapsed
        {
            get;
            set;
        }

        public KeyTrigger Trigger
        {
            get;
            private set;
        }
        public Key Keys
        {
            get;
            private set;
        }

        public Action Callback
        {
            get;
            private set;
        }
        #endregion

        public KeyboardBinding(string name, KeyTrigger trigger, Key keys, Action callback)
        {
            Name        = name;
            Trigger     = trigger;
            Keys        = keys;
            Callback    = callback;
        }
    }
}
