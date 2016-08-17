using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vRPGEngine.Input
{
    public enum KeyTrigger : int
    {
        Up = 0,         // Called when button is up.
        Released,       // Called when button is being released.
        Pressed,        // Called when button is pressed.
        Down            // Called when button is being held down.
    }

    public enum MouseTrigger : int
    {
        Up = 0,
        Relesed,
        Pressed,
        Down
    }

    [Flags()]
    public enum MouseButton : int 
    {
        None            = 0,
        LeftButton      = 1,
        RightButton     = 2,
        MiddleButton    = 4,
        XButton1        = 8,
        XButton2        = 16
    }
}
