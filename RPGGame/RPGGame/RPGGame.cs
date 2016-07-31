using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using vRPGEngine.Graphics;

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
            sprite  = new Sprite(Content.Load<Texture2D>("sprite"));
            view    = new View(GraphicsDevice.Viewport);

            var renderer = Renderer.Instance;
            renderer.SetPresentationParameters(1280, 720, 32, 32, 3, 3);

            var layer = renderer.CreateLayer();
            renderer.ShowLayer(layer);
            renderer.Add(sprite, layer);

            renderer.RegisterView(view);
        }

        protected override void Update(GameTime gameTime)
        {
            vRPGEngine.vRPGEngine.Instance.Update(gameTime);
        }
        protected override void Draw(GameTime gameTime)
        {
            vRPGEngine.vRPGEngine.Instance.Present(gameTime);
        }
    }
}
