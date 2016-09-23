using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using vRPGEngine.HUD;
using System.Diagnostics;
using vRPGEngine.HUD.Elements;
using vRPGEngine.HUD.Controls;
using vRPGEngine.HUD.Interfaces;
using vRPGEngine.Core;

namespace vRPGEngine.Graphics
{
    public sealed class DisplayInfo
    {
        public InfoLogEntry Entry;

        public int Elapsed;
    }

    public sealed class HUDRenderer : SystemManager<HUDRenderer>
    {
        #region Fields
        private readonly CombatTextManager combatText;

        private readonly SpriteBatch spriteBatch;

        private readonly List<IDisplayElement> elements;
         #endregion

        #region Properties
        public Vector2 CanvasSize
        {
            get
            {
                return new Vector2(Engine.Instance.GraphicsDeviceManager.PreferredBackBufferWidth,
                                   Engine.Instance.GraphicsDeviceManager.PreferredBackBufferHeight);
            }
        }
        #endregion

        private HUDRenderer()
            : base()
        {
            combatText  = new CombatTextManager();
            spriteBatch = new SpriteBatch(Engine.Instance.GraphicsDevice);
            elements    = new List<IDisplayElement>();
        }

        private void UpdateHUD(GameTime gameTime)
        {
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);

            if (HUDManager.Instance.Root != null) HUDManager.Instance.Root.Update(gameTime);
            
            for (int i = elements.Count - 1; i >= 0; i--) elements[i].Show(gameTime, spriteBatch);
            
            elements.Clear();

            spriteBatch.End();
        }
        private void UpdateSubsystems(GameTime gameTime)
        {
            if (HUDManager.Instance.Subsystems == null) return;

            var widgets = HUDManager.Instance.Subsystems;

            // Update first as some widgets might need to use 
            // the spritebatch in some different way that the 
            // rendering pipeline uses it.
            foreach (var widget in widgets) widget.Update(gameTime, spriteBatch);

            spriteBatch.Begin(SpriteSortMode.Deferred,
                              BlendState.AlphaBlend,
                              null,
                              null,
                              null,
                              null,
                              HUDManager.Instance.View.Transform);
            
            // Render all widgets after updates are done.
            foreach (var widget in widgets) widget.Draw(gameTime, spriteBatch);

            spriteBatch.End();
        }

        protected override void OnUpdate(GameTime gameTime)
        {
            if (!GameSetting.HUD.Visible) return;
            
            UpdateSubsystems(gameTime);
            UpdateHUD(gameTime);
            
            // Always clear log so we don't get a spam if the player 
            // changes the visibility of the HUD during the gameplay.
            GameInfoLog.Instance.Clear();
        }

        public void Show(IDisplayElement element)
        {
            Debug.Assert(element != null);

            elements.Add(element);
        }
    }
}
