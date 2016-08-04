using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TiledSharp;
using vRPGEngine;
using vRPGEngine.Graphics;
using vRPGEngine.Input;
using vRPGEngine.Maps;

namespace RPGGame
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class RPGGame : Game
    {
        #region Fields
        private readonly GraphicsDeviceManager graphics;

        private View view;
        private Sprite sprite;
        #endregion

        public RPGGame()
            : base()
        {
            graphics                            = new GraphicsDeviceManager(this);
            graphics.PreferMultiSampling        = true;
            graphics.PreferredBackBufferWidth   = 1280;
            graphics.PreferredBackBufferHeight  = 720;

            graphics.ApplyChanges();

            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            vRPGEngine.vRPGEngine.Instance.Initialize();

            sprite = new Sprite(DefaultValues.MissingTexture);
            
            view = new View(GraphicsDevice.Viewport);
            
            var renderer = Renderer.Instance;
            renderer.SetPresentationParameters(3200, 3200, 320, 320, 3, 3);

            var layer = renderer.CreateLayer();
            renderer.ShowLayer(layer);
            renderer.Add(sprite, layer);

            renderer.RegisterView(view);

            TileMapManager.Instance.Load("hello");

            var kb = InputManager.Instance.GetProvider<KeyboardInputProvider>();

            var camSpeed = 4.0f;
            var camZoom = 0.1f;

            kb.Bind("up", Keys.W, KeyTrigger.Down, () => view.Position += new Vector2(0.0f, -camSpeed));
            kb.Bind("down", Keys.S, KeyTrigger.Down, () => view.Position += new Vector2(0.0f, camSpeed));
            kb.Bind("left", Keys.A, KeyTrigger.Down, () => view.Position += new Vector2(-camSpeed, 0.0f));
            kb.Bind("right", Keys.D, KeyTrigger.Down, () => view.Position += new Vector2(camSpeed, 0.0f));

            kb.Bind("zoom_in", Keys.Q, KeyTrigger.Pressed, () => view.Zoom += camZoom);
            kb.Bind("zoom_out", Keys.E, KeyTrigger.Pressed, () => view.Zoom -= camZoom);
        }

        protected override void Update(GameTime gameTime)
        {
            vRPGEngine.vRPGEngine.Instance.Update(gameTime);
        }
    }
}
