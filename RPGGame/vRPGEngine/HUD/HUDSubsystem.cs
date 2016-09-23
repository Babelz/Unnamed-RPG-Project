using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vRPGEngine.Core;

namespace vRPGEngine.HUD
{
    public abstract class HUDSubsystem
    {
        #region Fields

        #endregion

        #region Properties
        public bool Visible
        {
            get;
            private set;
        }
        public bool Enabled
        {
            get;
            private set;
        }
        #endregion

        public HUDSubsystem()
        {
        }
        
        protected virtual void OnUpdate(GameTime gameTime, SpriteBatch spriteBatch)
        {
        }
        protected virtual void OnDraw(GameTime gameTime, SpriteBatch spriteBatch)
        {
        }

        protected virtual void OnShow()
        {
        }
        protected virtual void OnHide()
        {
        }
        protected virtual void OnEnable()
        {
        }
        protected virtual void OnDisable()
        {
        }

        public void Show()
        {
            if (Visible) return;

            OnShow();

            Visible = true;
        }
        public void Hide()
        {
            if (!Visible) return;

            OnHide();

            Visible = false;
        }

        public void Enable()
        {
            if (Enabled) return;

            OnEnable();

            Enabled = true;
        }
        public void Disable()
        {
            if (!Enabled) return;

            OnDisable();

            Enabled = false;
        }

        public void Update(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (!Enabled) return;

            OnUpdate(gameTime, spriteBatch);
        }
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (!Visible) return;

            OnDraw(gameTime, spriteBatch); 
        }
    }
}
