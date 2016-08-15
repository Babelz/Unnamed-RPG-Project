using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vRPGEngine.HUD
{
    public interface IDisplayElement
    {
        void Invalidate(Control control);

        void Show(GameTime gameTime, SpriteBatch spriteBatch);

        void SetValue(string name, object value);
    }
}
