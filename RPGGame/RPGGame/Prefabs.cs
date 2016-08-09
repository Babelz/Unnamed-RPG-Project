using FarseerPhysics;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vRPGEngine;
using vRPGEngine.Combat;
using vRPGEngine.Databases;
using vRPGEngine.ECS;
using vRPGEngine.ECS.Components;
using vRPGEngine.Graphics;
using vRPGEngine.Handlers.NPC;
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
            rendrer.Flags = RenderFlags.Anchored | RenderFlags.AutomaticDepth;

            rendrer.Sprite.Layer = Layers.Middle;
            rendrer.Sprite.ScaleTo(new Vector2(32.0f), rendrer.Sprite.Texture);

            var collider = player.AddComponent<BoxCollider>();
            collider.MakeDynamic(32.0f, 32.0f);
            collider.Category = Category.Cat1;
            collider.CollidesWith = Category.Cat2 | Category.Cat3;

            var behaviour = player.AddComponent<Behaviour>();

            var view         = new View(vRPGEngine.Engine.Instance.GraphicsDevice.Viewport);
            var zoomStep     = 0.1f;
            var controller   = player.AddComponent<CharacterController>();

            behaviour.Behave = new Action<GameTime>((gameTime) =>
            {
                collider.LinearVelocity = Vector2.Zero;

                view.Position = transform.Position;
                view.FocuCenter();
            });

            // Init input.
            var kip = InputManager.Instance.GetProvider<KeyboardInputProvider>();
            var velo = 2.0f;

            kip.Bind("player_up", Keys.Up, KeyTrigger.Down, () =>
            {
                collider.LinearVelocity = new Vector2(collider.LinearVelocity.X, -velo);
            });

            kip.Bind("player_down", Keys.Down, KeyTrigger.Down, () =>
            {
                collider.LinearVelocity = new Vector2(collider.LinearVelocity.X, velo);
            });

            kip.Bind("player_left", Keys.Left, KeyTrigger.Down, () =>
            {
                collider.LinearVelocity = new Vector2(-velo, collider.LinearVelocity.Y);
            });

            kip.Bind("player_right", Keys.Right, KeyTrigger.Down, () =>
            {
                collider.LinearVelocity = new Vector2(velo, collider.LinearVelocity.Y);
            });
            
            kip.Bind("zoom_in", Keys.Q, KeyTrigger.Pressed, () => view.Zoom += zoomStep);
            kip.Bind("zoom_out", Keys.E, KeyTrigger.Pressed, () => view.Zoom -= zoomStep);

            var mip = InputManager.Instance.GetProvider<MouseInputProvider>();

            mip.Bind("click", MouseButton.LeftButton, MouseTrigger.Pressed, (ms) =>
            {
                var position = view.ScreenToView(ms.Position);
                var radius   = 2.5f;

                targetFinder.FindTarget(position, radius);
            });
            
            Renderer.Instance.RegisterView(view);

            return player;
        }

        private static Entity SheepPrefab()
        {
            var sheep       = Entity.Create();

            var width       = RPGWorld.UnitToPixel * 1.3f;
            var height      = RPGWorld.UnitToPixel * 0.7f;
            
            var transform   = sheep.AddComponent<Transform>();

            var collider    = sheep.AddComponent<BoxCollider>();
            collider.MakeDynamic(width, height);
            collider.Category = Category.Cat1;
            collider.CollidesWith = Category.Cat2 | Category.Cat3;

            var controller  = sheep.AddComponent<NPCController>();
            
            var data        = NPCDatabase.Instance.Elements().FirstOrDefault(n => n.ID == 0);

            var handler     = NPCHandlerFactory.Instance.Create(data, data.HandlerName);
            handler.Owner   = sheep;
            handler.Data    = data;

            controller.Initialize(handler);

            var rendrer             = sheep.AddComponent<SpriteRenderer>();
            rendrer.Flags = RenderFlags.Anchored | RenderFlags.AutomaticDepth;
            rendrer.Sprite.Layer    = Layers.Middle;
            rendrer.Sprite.Texture = Engine.Instance.Content.Load<Texture2D>("sheep");
            rendrer.Sprite.Source = new Rectangle(0, 0, 32, 32);
            
            return sheep;
        }

        public static void Register()
        {
            var builder = EntityBuilder.Instance;

            builder.RegisterPrefab("player", PlayerPrefab);
            builder.RegisterPrefab("sheep", SheepPrefab);
        }
    }
}
