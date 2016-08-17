using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using vRPGEngine.HUD.Controls;
using vRPGEngine.Handlers.Spells;
using vRPGContent.Data.Spells;

namespace vRPGEngine.HUD.Elements
{
    public sealed class BindButtonElement : IDisplayElement
    {
        #region Fields
        private Vector2 position;
        private Vector2 scale;

        private MouseHoverState hoverState;
        
        private SpellHandler handler;
        private Spell spell;
        #endregion

        public BindButtonElement()
        {
        }

        public void Invalidate(Control control)
        {
            if (!control.ReadProperty("Spell", ref spell)) return;

            if (!control.ReadProperty("Handler", ref handler)) spell = null;

            position = control.DisplayPosition;
            scale    = control.DisplaySize / 32.0f;
        }

        public void Show(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (spell != null)
            {
                spriteBatch.Draw(Engine.Instance.Content.Load<Texture2D>(spell.IconName),
                                 position,
                                 null,
                                 null,
                                 null,
                                 0.0f,
                                 scale,
                                 Color.White,
                                 SpriteEffects.None,
                                 0.0f);

                if (hoverState == MouseHoverState.Hover)
                {
                    // Draw info.    
                }
            }
        }
    }
}
