using FarseerPhysics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vRPGEngine.ECS;
using vRPGEngine.ECS.Components;
using vRPGEngine.Graphics;
using vRPGEngine.Input;

namespace RPGGame
{
    public static class Prefabs
    {
        private static Entity PlayerPrefab()
        {
            var player  = Entity.Create();
            player.Tags = "player";

            var transform = player.AddComponent<Transform>();

            var rendrer = player.AddComponent<SpriteRenderer>();
            rendrer.Sprite.Layer = Layers.EntityLayerBottom;
            rendrer.Sprite.ScaleTo(new Vector2(32.0f), rendrer.Sprite.Texture);

            rendrer.Anchored = true;

            var collider = player.AddComponent<BoxCollider>();
            collider.MakeDynamic(32.0f, 32.0f);

            var behaviour = player.AddComponent<Behaviour>();

            var view     = new View(vRPGEngine.vRPGEngine.Instance.GraphicsDevice.Viewport);
            var zoomStep = 0.1f;

            behaviour.Behave = new Action<GameTime>((gameTime) =>
            {
                collider.LinearVelocity = Vector2.Zero;

                view.Position = transform.Position;
                view.FocuCenter();
            });

            // Init input.
            var kb = InputManager.Instance.GetProvider<KeyboardInputProvider>();
            var velo = 2.0f;

            kb.Bind("player_up", Keys.Up, KeyTrigger.Down, () =>
            {
                collider.LinearVelocity = new Vector2(collider.LinearVelocity.X, -velo);
            });

            kb.Bind("player_down", Keys.Down, KeyTrigger.Down, () =>
            {
                collider.LinearVelocity = new Vector2(collider.LinearVelocity.X, velo);
            });

            kb.Bind("player_left", Keys.Left, KeyTrigger.Down, () =>
            {
                collider.LinearVelocity = new Vector2(-velo, collider.LinearVelocity.Y);
            });

            kb.Bind("player_right", Keys.Right, KeyTrigger.Down, () =>
            {
                collider.LinearVelocity = new Vector2(velo, collider.LinearVelocity.Y);
            });
            
            kb.Bind("zoom_in", Keys.Q, KeyTrigger.Pressed, () => view.Zoom += zoomStep);
            kb.Bind("zoom_out", Keys.E, KeyTrigger.Pressed, () => view.Zoom -= zoomStep);
            
            Renderer.Instance.RegisterView(view);

            return player;
        }

        public static void Register()
        {
            var builder = vRPGEngine.ECS.EntityBuilder.Instance;

            builder.RegisterPrefab("player", PlayerPrefab);
        }
    }
}
