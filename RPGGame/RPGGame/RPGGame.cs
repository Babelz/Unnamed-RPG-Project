using FarseerPhysics;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TiledSharp;
using vRPGEngine;
using vRPGEngine.Databases;
using vRPGEngine.ECS;
using vRPGEngine.ECS.Components;
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
        #endregion

        public RPGGame()
            : base()
        {
            graphics                            = new GraphicsDeviceManager(this);
            graphics.PreferMultiSampling        = true;
            graphics.PreferredBackBufferWidth   = 1280;
            graphics.PreferredBackBufferHeight  = 720;

            graphics.ApplyChanges();
            
            Content.RootDirectory   = "Content";
            IsMouseVisible          = true;
        }

        protected override void Initialize()
        {
            vRPGEngine.Engine.Instance.Initialize();
        }

        protected override void Update(GameTime gameTime)
        {
            vRPGEngine.Engine.Instance.Update(gameTime);
        }
    }
}
