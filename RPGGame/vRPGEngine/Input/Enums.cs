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
}
