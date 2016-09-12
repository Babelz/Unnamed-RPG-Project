using FarseerPhysics;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using RPGGame.Scenes;
using vRPGEngine;
using vRPGEngine.Databases;
using vRPGEngine.ECS;
using vRPGEngine.ECS.Components;
using vRPGEngine.Graphics;
using vRPGEngine.Input;
using vRPGEngine.Maps;
using vRPGEngine.Scenes;
using System.Linq;
using vRPGEngine.Handlers.NPC;
using vRPGEngine.Handlers.Spells;

namespace RPGGame
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public sealed class RPGGame : Game
    {
        #region Fields
        private GraphicsDeviceManager graphics;
        #endregion

        public RPGGame()
            : base()
        {
            graphics = new GraphicsDeviceManager(this);
        }

        protected override void Initialize()
        {
            Content.RootDirectory   = "Content";
            IsMouseVisible          = true;

            Engine.Instance.Initialize(graphics);

            SceneManager.Instance.ChangeScene(new GameplayTestingScene());
        }

        protected override void Update(GameTime gameTime)
        {
            Engine.Instance.Update(gameTime);
        }
    }
}