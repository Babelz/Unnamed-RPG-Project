using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vRPGEngine.HUD.Controls;

namespace vRPGEngine.HUD.Elements
{
    public interface IDisplayElement
    {
        void Invalidate(Control control);

        void Show(GameTime gameTime, SpriteBatch spriteBatch);
    }
}
