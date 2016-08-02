using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using vRPGEngine.Graphics;
using vRPGEngine.Input;

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
        {
            graphics = new GraphicsDeviceManager(this);

            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            vRPGEngine.vRPGEngine.Instance.Initialize();

            sprite  = new Sprite(Content.Load<Texture2D>("sprite"));
            view    = new View(GraphicsDevice.Viewport);

            var renderer = Renderer.Instance;
            renderer.SetPresentationParameters(1280, 720, 32, 32, 3, 3);

            var layer = renderer.CreateLayer();
            renderer.ShowLayer(layer);
            renderer.Add(sprite, layer);

            renderer.RegisterView(view);
            
            var kb = InputManager.Instance.GetProvider<KeyboardInputProvider>();

            kb.Bind("up", Keys.W, KeyTrigger.Up, Up);
            kb.Bind("released", Keys.A, KeyTrigger.Released, Released);
            kb.Bind("down", Keys.S, KeyTrigger.Down, Down);
            kb.Bind("pressed", Keys.D, KeyTrigger.Pressed, Pressed);
        }

        private static void Up()
        {
            System.Console.WriteLine("up");
        }
        private static void Released()
        {
            System.Console.WriteLine("released");
        }
        private static void Down()
        {
            System.Console.WriteLine("down");
        }
        private static void Pressed()
        {
            System.Console.WriteLine("pressed");
        }

        protected override void Update(GameTime gameTime)
        {
            vRPGEngine.vRPGEngine.Instance.Update(gameTime);
        }
    }
}
