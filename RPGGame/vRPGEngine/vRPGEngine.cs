using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public void Exit()
        {
            game.Exit();
        }
    }
}
