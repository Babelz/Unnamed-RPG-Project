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
using vRPGEngine.Input;

namespace vRPGEngine
{
    public sealed class vRPGEngine : Singleton<vRPGEngine>
    {
        #region Fields
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
        
        public bool Initialize()
        {
            try
            {
                InputManager.Instance.Activate();
                EntityManager.Instance.Activate();
                Renderer.Instance.Activate();
                Logger.Instance.Activate();

                Logger.Instance.LogFunctionMessage("engine systems initialized ok!");

                return true;
            }
            catch (Exception e)
            {
                Logger.Instance.LogError("could not initialize all systems!");
                Logger.Instance.LogError("and exceptin was thrown");
                Logger.Instance.LogError("exception message: " + e.Message);

                return false;
            }
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
            
            game.Run();
        }

        public void Update(GameTime gameTime)
        {
            SystemManagers.Instance.Update(gameTime);
        }

        public void Exit()
        {
            game.Exit();
        }
    }
}
