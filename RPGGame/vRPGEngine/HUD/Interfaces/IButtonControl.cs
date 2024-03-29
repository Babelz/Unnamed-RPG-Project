﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vRPGEngine.HUD.Controls;
using vRPGEngine.Input;

namespace vRPGEngine.HUD.Interfaces
{
    public delegate void ButtonMouseEventHandler(IButtonControl sender);
    
    public interface IButtonControl : IMouseControl
    {
        #region Events
        event ButtonMouseEventHandler ButtonDown;
        event ButtonMouseEventHandler ButtonUp;

        event ButtonMouseEventHandler ButtonPressed;
        event ButtonMouseEventHandler ButtonReleased;
        #endregion
    }
}
