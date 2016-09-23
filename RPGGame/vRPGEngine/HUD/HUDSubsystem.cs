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

        protected virtual void OnShow(ref bool show)
        {
        }
        protected virtual void OnHide(ref bool hide)
        {
        }
        protected virtual void OnEnable(ref bool enable)
        {
        }
        protected virtual void OnDisable(ref bool disable)
        {
        }

        public void Show()
        {
            if (Visible) return;

            var results = false;

            OnShow(ref results);

            if (!results) Logger.Instance.LogWarning("could not show HUD subsystem!");

            Visible = results;
        }
        public void Hide()
        {
            if (!Visible) return;

            var results = false;

            OnHide(ref results);

            if (!results) Logger.Instance.LogWarning("could not hide HUD subsystem!");

            Visible = results;
        }

        public void Enable()
        {
            if (Enabled) return;

            var results = false;

            OnEnable(ref results);

            if (!results) Logger.Instance.LogWarning("could not enable HUD subsystem!");

            Enabled = results;
        }
        public void Disable()
        {
            if (!Enabled) return;

            var results = false;

            OnDisable(ref results);

            if (!results) Logger.Instance.LogWarning("could not disable HUD subsystem!");

            Enabled = results;
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
