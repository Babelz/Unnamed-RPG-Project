﻿using FarseerPhysics;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TiledSharp;
using vRPGEngine;
using vRPGEngine.Databases;
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

        Body body;

        protected override void Initialize()
        {
            vRPGEngine.vRPGEngine.Instance.Initialize();

            sprite = new Sprite(DefaultValues.MissingTexture);
            sprite.Depth = 0.0f;
            
            view = new View(GraphicsDevice.Viewport);
            
            var renderer = Renderer.Instance;
            renderer.SetPresentationParameters(3200, 3200, 320, 320, 3, 3);
            
            renderer.RegisterView(view);
            
            var layer = renderer.CreateLayer();

            TileMapManager.Instance.Load("hello");
            
            renderer.ShowLayer(layer);
            renderer.Add(sprite, layer);

            var kb = InputManager.Instance.GetProvider<KeyboardInputProvider>();

            var camSpeed = 4.0f;
            var camZoom = 0.1f;

            kb.Bind("up", Keys.W, KeyTrigger.Down, () => view.Position += new Vector2(0.0f, -camSpeed));
            kb.Bind("down", Keys.S, KeyTrigger.Down, () => view.Position += new Vector2(0.0f, camSpeed));
            kb.Bind("left", Keys.A, KeyTrigger.Down, () => view.Position += new Vector2(-camSpeed, 0.0f));
            kb.Bind("right", Keys.D, KeyTrigger.Down, () => view.Position += new Vector2(camSpeed, 0.0f));

            kb.Bind("zoom_in", Keys.Q, KeyTrigger.Pressed, () => view.Zoom += camZoom);
            kb.Bind("zoom_out", Keys.E, KeyTrigger.Pressed, () => view.Zoom -= camZoom);

            var force = 2.5f;

            kb.Bind("player_up", Keys.Up, KeyTrigger.Down, () => body.LinearVelocity = new Vector2(body.LinearVelocity.X, -force));
            kb.Bind("player_down", Keys.Down, KeyTrigger.Down, () => body.LinearVelocity = new Vector2(body.LinearVelocity.X, force));
            kb.Bind("player_left", Keys.Left, KeyTrigger.Down, () => body.LinearVelocity = new Vector2(-force, body.LinearVelocity.Y));
            kb.Bind("player_right", Keys.Right, KeyTrigger.Down, () => body.LinearVelocity = new Vector2(force, body.LinearVelocity.Y));

            body = RPGWorld.Instance.CreateEntityCollider(renderer, 32.0f, 32.0f);
            RPGWorld.Instance.Activate();

            var specializations = SpecializationDatabase.Instance;
        }

        protected override void Update(GameTime gameTime)
        {
            vRPGEngine.vRPGEngine.Instance.Update(gameTime);

            sprite.Position = ConvertUnits.ToDisplayUnits(body.Position);

            body.LinearVelocity = Vector2.Zero;
        }
    }
}
