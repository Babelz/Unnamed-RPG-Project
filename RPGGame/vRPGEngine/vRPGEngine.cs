using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vRPGEngine.ECS;
using vRPGEngine.Graphics;

namespace vRPGEngine
{
    public sealed class vRPGEngine : Singleton<vRPGEngine>
    {
        #region Fields
        private SpriteBatch spriteBatch;

        private Game game;
        #endregion

        #region Properties
        public bool Running
        {
            get
            {
                return game.IsActive;
            }
        }
        public ContentManager Content
        {
            get
            {
                return game.Content;
            }
        }
        public GraphicsDevice GraphicsDevice
        {
            get
            {
                return game.GraphicsDevice;
            }
        }
        public GameWindow GameWindow
        {
            get
            {
                return game.Window;
            }
        }
        #endregion

        private vRPGEngine() 
            : base()
        {
        }
        
        private void ActivateECS()
        {
            Logger.Instance.LogFunctionMessage("activating ECS...");
        }
        private void ActivateRenderingSystem()
        {
            Logger.Instance.LogFunctionMessage("activating render system...");
        }
        private void ActivateDataSystem()
        {
            Logger.Instance.LogFunctionMessage("activating data systems...");
        }
        private void ActivateRenderer()
        {
            Logger.Instance.LogFunctionMessage("activating renderer system...");
        }

        public void InsertGame(Game game)
        {
            Debug.Assert(game != null);

            this.game = game;
        }

        public void Start()
        {
            Debug.Assert(game != null);
            Debug.Assert(!game.IsActive);

            Logger.Instance.LogFunctionMessage("activating engine systems...");

            try
            {
                ActivateECS();
                ActivateRenderingSystem();
                ActivateDataSystem();
                
                Logger.Instance.LogFunctionMessage("engine systems initialized ok!");
            }
            catch (Exception e)
            {
                Logger.Instance.LogError("could not initialize all systems!");
                Logger.Instance.LogError("and exceptin was thrown");
                Logger.Instance.LogError("exception message: " + e.Message);

                return;
            }
            
            game.Run();
        }

        public void Update(GameTime gameTime)
        {
            ComponentManagers.Instance.Update(gameTime);
        }
        public void Present(GameTime gameTime)
        {
            Renderer.Instance.Present();
        }

        public void Exit()
        {
            game.Exit();
        }
    }
}
