using OpenTK.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MouseState    = Microsoft.Xna.Framework.Input.MouseState;

namespace vRPGEngine.Input
{
    public sealed class MouseBinding
    {
        #region Properties
        public string Name
        {
            get;
            private set;
        }

        public MouseTrigger Trigger
        {
            get;
            private set;
        }
        public MouseButton Buttons
        {
            get;
            private set;
        }

        public ButtonState LastState
        {
            get;
            set;
        }

        public Action<MouseState> Callback
        {
            get;
            private set;
        }
        #endregion

        public MouseBinding(string name, MouseTrigger triggers, MouseButton buttons, Action<MouseState> callback)
        {
            Name        = name;
            Trigger     = triggers;
            Buttons     = buttons;
            Callback    = callback;
        }
    }
}