using FarseerPhysics;
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
using vRPGEngine.HUD;
using vRPGEngine.Input;
using vRPGEngine.Scenes;

namespace vRPGEngine
{
    public sealed class Engine : Singleton<Engine>
    {
        #region Fields
        private GraphicsDeviceManager graphics;

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
        public GraphicsDeviceManager GraphicsDeviceManager
        {
            get
            {
                return graphics;
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

        private Engine() 
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
            var last    = string.Empty;
            var elapsed = 0;
            DebugRenderer.Instance.AddString((gt) =>
            {
                elapsed += gt.ElapsedGameTime.Milliseconds;

                if (elapsed > 160)
                {
                    last    = string.Format("Memory used: {0}kb", GC.GetTotalMemory(false) / 1024);
                    elapsed = 0;
                }

                return last;
            });

            DebugRenderer.Instance.AddString((gt) =>
            {
                var player = EntityManager.Instance.Entitites.FirstOrDefault(e => e.Tags == "player");

                if (player == null) return string.Empty;

                var controller = player.FirstComponentOfType<ICharacterController>();

                if (controller.TargetFinder.Target == null) return "Dist: null";

                var pos  = controller.TargetFinder.Target.FirstComponentOfType<Transform>().Position;
                var dist = ConvertUnits.ToSimUnits(Vector2.Distance(player.FirstComponentOfType<Transform>().Position, pos));

                return "Dist: " + Math.Round(dist, 2);
            });

            DebugRenderer.Instance.AddString((gt) =>
            {
                var player = EntityManager.Instance.Entitites.FirstOrDefault(e => e.Tags == "player");

                if (player == null) return string.Empty;

                var controller = player.FirstComponentOfType<ICharacterController>();

                if (controller.TargetFinder.Target == null) return "Target HP: null";

                return "Target HP: " + controller.TargetFinder.TargetController.Statuses.Health;
            });

            DebugRenderer.Instance.AddString((gt) =>
            {
                var player = EntityManager.Instance.Entitites.FirstOrDefault(e => e.Tags == "player");

                if (player == null) return string.Empty;

                var controller = player.FirstComponentOfType<ICharacterController>();
                
                return "HP: " + controller.Statuses.Health + "/" + controller.Specialization.TotalHealth();
            });

            DebugRenderer.Instance.AddString((gt) =>
            {
                var player = EntityManager.Instance.Entitites.FirstOrDefault(e => e.Tags == "player");

                if (player == null) return string.Empty;

                var controller = player.FirstComponentOfType<ICharacterController>();

                return "Focus: " + controller.Statuses.Focus + "/" + controller.Specialization.TotalFocus();
            });

            DebugRenderer.Instance.AddString((gt) =>
            {
                var player = EntityManager.Instance.Entitites.FirstOrDefault(e => e.Tags == "player");

                if (player == null) return string.Empty;

                var controller = player.FirstComponentOfType<ICharacterController>();

                return "Melee power: " + controller.Specialization.TotalMeleePower();
            });
        }
        
        public bool Initialize(GraphicsDeviceManager graphics)
        {
            Debug.Assert(game != null);
            Debug.Assert(graphics != null);

            this.graphics = graphics;

            try
            {
                // TODO: wrap.
                // Init managers.
                ComponentManager<DataDictionary>.Instance.Activate();

                ComponentManager<SpriteRenderer>.Instance.Activate();
                ComponentManager<SpriteRenderer>.Instance.SetUpdateHandler(new SpriteHandler());

                ComponentManager<Behaviour>.Instance.Activate();
                ComponentManager<Behaviour>.Instance.SetUpdateHandler(new BehaviourHandler());

                ComponentManager<Transform>.Instance.Activate();
                ComponentManager<Transform>.Instance.SetUpdateHandler(new TransformHandler());

                ComponentManager<SpawnArea>.Instance.Activate();
                ComponentManager<SpawnArea>.Instance.SetUpdateHandler(new SpawnAreaHandler());
                
                ComponentManager<PlayerCharacterController>.Instance.Activate();
                ComponentManager<PlayerCharacterController>.Instance.SetUpdateHandler(new PlayerCharacterControllerHandler());

                ComponentManager<NPCController>.Instance.Activate();
                ComponentManager<NPCController>.Instance.SetUpdateHandler(new NPCControllerHandler());

                // TODO: wrap.
                // Init systems.
                InputManager.Instance.Activate();
                EntityManager.Instance.Activate();
                Renderer.Instance.Activate();
                Logger.Instance.Activate();
                RPGWorld.Instance.Activate();
                HUDInputManager.Instance.Activate();
                HUDRenderer.Instance.Activate();
                SceneManager.Instance.Activate();

                ActivateDebugRenderer();

                Logger.Instance.LogFunctionMessage("engine systems initialized ok!");

                return true;
            }
            catch (Exception e)
            {
                Logger.Instance.LogFunctionError("could not initialize all systems!");
                Logger.Instance.LogFunctionError("and exceptin was thrown");
                Logger.Instance.LogFunctionError("exception message: " + e.Message);

                Logger.Instance.Update(null);

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
            Debug.Assert(game != null);

            SystemManagers.Instance.Update(gameTime);

            DebugRenderer.Instance.Present(gameTime);
        }

        public void Exit()
        {
            game.Exit();
        }
    }
}
