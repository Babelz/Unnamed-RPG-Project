using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vRPGEngine.HUD.Controls;

namespace vRPGEngine.HUD.Elements
{
    public interface IConentDisplayElement : IDisplayElement
    {
        void SetContent(object content, Control sender);
    }
}
