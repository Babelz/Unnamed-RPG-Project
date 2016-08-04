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
using vRPGEngine.ECS.Components;
using vRPGEngine.ECS.Handlers;
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

        private void ActivateDebugRenderer()
        {
            // Delta.
            DebugRenderer.Instance.AddString((gt) => string.Format("Delta time: {0}ms", gt.ElapsedGameTime.Milliseconds));

            // Render info.
            DebugRenderer.Instance.AddString((gt) => string.Format("Elements renderer: {0}/{1}", Renderer.Instance.VisibleElements, Renderer.Instance.TotalElements));

            // FPS.
            var fps = new FrameCounter();
            DebugRenderer.Instance.AddString((gt) => { fps.Update(gt); return string.Format("Current FPS: {0}", Math.Round(fps.CurrentFramesPerSecond, 2)); });
            DebugRenderer.Instance.AddString((gt) => { fps.Update(gt); return string.Format("Average FPS: {0}", Math.Round(fps.AverageFramesPerSecond, 2)); });

            // Memory.
            DebugRenderer.Instance.AddString((gt) => string.Format("Memory used: {0}kb", GC.GetTotalMemory(false) / 1024));
        }
        
        public bool Initialize()
        {
            try
            {
                InputManager.Instance.Activate();

                ComponentManager<DataDictionary>.Instance.Activate();

                ComponentManager<SpriteRenderer>.Instance.Activate();
                ComponentManager<SpriteRenderer>.Instance.SetUpdateHandler(new SpriteHandler());

                ComponentManager<Behaviour>.Instance.Activate();
                ComponentManager<Behaviour>.Instance.SetUpdateHandler(new BehaviourHandler());

                EntityManager.Instance.Activate(); ;
                Renderer.Instance.Activate();
                Logger.Instance.Activate();

                ActivateDebugRenderer();

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

            DebugRenderer.Instance.Present(gameTime);
        }

        public void Exit()
        {
            game.Exit();
        }
    }
}
