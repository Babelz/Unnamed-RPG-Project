﻿using FarseerPhysics;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using RPGGame.Scenes;
using TiledSharp;
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
    public class RPGGame : Game
    {
        #region Fields
        private readonly GraphicsDeviceManager graphics;
        #endregion

        public RPGGame()
            : base()
        {
            graphics                                = new GraphicsDeviceManager(this);
            graphics.SynchronizeWithVerticalRetrace = true;
            graphics.PreferMultiSampling            = false;
            graphics.PreferredBackBufferWidth       = 1280;
            graphics.PreferredBackBufferHeight      = 720;

            graphics.ApplyChanges();
            
            Content.RootDirectory   = "Content";
            IsMouseVisible          = true;
        }

        protected override void Initialize()
        {
            Engine.Instance.Initialize(graphics);

            NPCHandlerFactory.Instance.HandlersNamespace    = "RPGGame.Handlers.NPC.";
            SpellHandlerFactory.Instance.HandlersNamespace  = "RPGGame.Handlers.Spells.";

            SceneManager.Instance.ChangeScene(new GameplayTestingScene());
        }

        protected override void Update(GameTime gameTime)
        {
            Engine.Instance.Update(gameTime);
        }
    }
}