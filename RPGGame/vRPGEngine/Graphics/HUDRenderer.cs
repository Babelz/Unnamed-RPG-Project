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
        private readonly SpriteBatch spriteBatch;

        private readonly List<IDisplayElement> elements;

        private int elapsed;
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
        public IContentControl Root
        {
            get;
            set;
        }
        #endregion

        private HUDRenderer()
            : base()
        {
            spriteBatch = new SpriteBatch(Engine.Instance.GraphicsDevice);
            elements    = new List<IDisplayElement>();
        }

        private void DrawHUD(GameTime gameTime)
        {
            for (int i = elements.Count - 1; i >= 0; i--) elements[i].Show(gameTime, spriteBatch);

            elements.Clear();
        }
        private void DrawCombatText(GameTime gameTime)
        {
        }

        protected override void OnUpdate(GameTime gameTime)
        {
            if (!GameSetting.HUDVisible) return;

            if (Root != null) Root.Update(gameTime);

            spriteBatch.Begin();

            DrawCombatText(gameTime);
            DrawHUD(gameTime);

            spriteBatch.End();
        }

        public void Show(IDisplayElement element)
        {
            Debug.Assert(element != null);

            elements.Add(element);
        }
    }
}
