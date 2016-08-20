using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vRPGEngine.HUD.Controls;

namespace vRPGEngine.HUD.Interfaces
{
    public delegate void MouseControlEventHandler(IMouseControl sender);

    public interface IMouseControl
    {
        #region Events
        event MouseControlEventHandler OnMouseEnter;
        event MouseControlEventHandler OnMouseLeave;
        event MouseControlEventHandler OnMouseHover;
        #endregion
    }
}
