using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vRPGEngine.HUD.Controls;
using vRPGEngine.HUD.Elements;

namespace vRPGEngine.HUD.Interfaces
{
    public interface IScrollViewElement : IDisplayElement
    {
        #region Properties
        Vector2 Overlap
        {
            get;
        }
        #endregion

        void SetScroll(Vector2 scroll, Control sender);
    }
}
