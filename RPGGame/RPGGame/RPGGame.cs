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

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // TODO: use this.Content to load your game content here
            sprite = new Sprite(Content.Load<Texture2D>("sprite"));
            view = new View(GraphicsDevice.Viewport);
            
            var renderer = Renderer.Instance;
            renderer.SetPresentationParameters(1280, 720, 32, 32, 3, 3);

            var layer = renderer.CreateLayer();
            renderer.ShowLayer(layer);
            renderer.Add(sprite, layer);

            renderer.RegisterView(view);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            vRPGEngine.vRPGEngine.Instance.Update(gameTime);

            base.Update(gameTime);
        }

        SpriteBatch sb;

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            // TODO: Add your drawing code here
            vRPGEngine.vRPGEngine.Instance.Present(gameTime);

            //sprite.Position = sprite.Position + new Vector2(1.0f);
            //Renderer.Instance.Invalidate(sprite);
            //GraphicsDevice.Clear(Color.CornflowerBlue);

            //if (sb == null) sb = new SpriteBatch(GraphicsDevice);

            //sb.Begin();
            //sb.Draw(Content.Load<Texture2D>("sprite"), new Rectangle(0, 0, 32, 32), Color.White);
            //sb.End();

            base.Draw(gameTime);
        }
    }
}
