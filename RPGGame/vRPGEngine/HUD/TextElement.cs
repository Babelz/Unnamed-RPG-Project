using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace vRPGEngine.HUD
{
    public sealed class TextElement : IDisplayElement
    {
        public TextElement()
        {
        }

        public void Invalidate(Control control)
        {
            throw new NotImplementedException();
        }

        public void SetValue(string name, object value)
        {
            throw new NotImplementedException();
        }

        public void Show(GameTime gameTime, SpriteBatch spriteBatch)
        {
            throw new NotImplementedException();
        }
    }
}
