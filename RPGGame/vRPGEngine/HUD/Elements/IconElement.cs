using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using vRPGEngine.HUD.Controls;
using vRPGContent.Data.Spells;
using vRPGContent.Data.Items;
using vRPGEngine.Handlers.Spells;

namespace vRPGEngine.HUD.Elements
{
    public sealed class IconElement : IDisplayElement
    {
        #region Fields
        private SpellHandler handler;
        private Spell spell;

        private Item item;
        #endregion

        public void Invalidate(Control control)
        {
            throw new NotImplementedException();
        }

        public void Show(GameTime gameTime, SpriteBatch spriteBatch)
        {
            throw new NotImplementedException();
        }
    }
}
