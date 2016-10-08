using FarseerPhysics;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Penumbra;
using RPGGame.Specializations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vRPGContent.Data.Attributes;
using vRPGEngine;
using vRPGEngine.Attributes;
using vRPGEngine.Attributes.Specializations;
using vRPGEngine.Combat;
using vRPGEngine.Databases;
using vRPGEngine.ECS;
using vRPGEngine.ECS.Components;
using vRPGEngine.Graphics;
using vRPGEngine.Handlers.NPC;
using vRPGEngine.Handlers.Spells;
using vRPGEngine.HUD;
using vRPGEngine.Input;
using vRPGEngine.Maps;

namespace RPGGame
{
    public static class Prefabs
    {
        private static Entity PlayerPrefab()
        {
            var player  = Entity.Create();

            player.Tag("player");

            var transform = player.AddComponent<Transform>();

            var renderer   = player.AddComponent<SpriteRenderer>();
            renderer.Flags = RenderFlags.Anchored | RenderFlags.AutomaticDepth;

            renderer.Sprite.Layer = Layers.Middle;
            renderer.Sprite.ScaleTo(new Vector2(32.0f), renderer.Sprite.Texture);

            var collider             = player.AddComponent<Collider>();
            collider.MakeDynamicBox(32.0f, 32.0f);
            collider.Category        = CollisionCategories.Entitites;
            collider.CollidesWith    = CollisionCategories.World;
            collider.DisplayPosition = new Vector2(100.0f, 100.0f);

            var camera              = player.AddComponent<Camera>();
            var zoomStep            = 0.1f;

            camera.Zoom             += zoomStep * 5.0f;

            var behaviour = player.AddComponent<Behaviour>();

            // TODO: load from game data.
            var data                  = SpecializationDatabase.Instance.Elements().First(s => s.Name.ToLower() == "warrior");
            var controller            = player.AddComponent<PlayerCharacterController>();
            var attributes            = new AttributesData()
            {
                Level = 10,
                Fp5 = 25,
            };

            var equipments            = new EquipmentContainer()
            {
                MainHand = WeaponDatabase.Instance.Elements().First()
            };

            var meleeDamageController   = new MeleeDamageController();
            var rangedDamageController  = new RangedDamageController();
            var statuses                = new Statuses();
            var specialization          = new Mage(attributes, equipments, statuses, rangedDamageController);
            
            meleeDamageController.Initialize(equipments, specialization);
            rangedDamageController.Initialize(specialization);
            statuses.Initialize(specialization);
            controller.Initialize(specialization, attributes, equipments, statuses, meleeDamageController, rangedDamageController);

            var light           = Renderer.Instance.CreatePointLight(ShadowType.Illuminated);
            Renderer.Instance.AddLight(light);
            light.Scale         = new Vector2(400.0f);
            light.Radius        = 32.0f;
            light.Intensity     = 1.0f;
            light.Color         = Color.White;
            light.Enabled       = true;
            light.CastsShadows  = true;

            behaviour.Behave = new Action<GameTime>((gameTime) =>
            {
                collider.LinearVelocity = Vector2.Zero;

                Vector2 viewPosition;
                Vector2 viewSize    = camera.VisibleArea;
                viewPosition.X      = MathHelper.Clamp(collider.DisplayPosition.X, viewSize.X * 0.5f, TileEngine.TileWidth * TileEngine.MapWidth - viewSize.X * 0.5f);
                viewPosition.Y      = MathHelper.Clamp(collider.DisplayPosition.Y, viewSize.Y * 0.5f, TileEngine.TileHeight * TileEngine.MapHeight - viewSize.Y * 0.5f);
                camera.Position     = viewPosition;

                light.Position      = collider.DisplayPosition;

                light.Scale         = new Vector2(400.0f) * vRPGRandom.NextFloat(0.8f, 1.0f);

                camera.FocuCenter();
            });

            // Init input.
            var kip = InputManager.Instance.GetProvider<KeyboardInputProvider>();
            var velo = 1.0f;

            kip.Bind("player_up", Keys.W, KeyTrigger.Down, () =>
            {
                collider.LinearVelocity = new Vector2(collider.LinearVelocity.X, -velo);
            });

            kip.Bind("player_down", Keys.S, KeyTrigger.Down, () =>
            {
                collider.LinearVelocity = new Vector2(collider.LinearVelocity.X, velo);
            });

            kip.Bind("player_left", Keys.A, KeyTrigger.Down, () =>
            {
                collider.LinearVelocity = new Vector2(-velo, collider.LinearVelocity.Y);
            });

            kip.Bind("player_right", Keys.D, KeyTrigger.Down, () =>
            {
                collider.LinearVelocity = new Vector2(velo, collider.LinearVelocity.Y);
            });

            kip.Bind("zoom_in", Keys.Q, KeyTrigger.Pressed, () => camera.Zoom += zoomStep);
            kip.Bind("zoom_out", Keys.E, KeyTrigger.Pressed, () => camera.Zoom -= zoomStep);

            var mip = InputManager.Instance.GetProvider<MouseInputProvider>();

            mip.Bind("click", MouseButton.LeftButton, MouseTrigger.Pressed, (ms) =>
            {
                var position = camera.ScreenToCamera(ms.Position);
                var radius   = 2.5f;

                controller.TargetFinder.FindTarget(position, radius);
            });
            
            return player;
        }

        private static Entity SheepPrefab()
        {
            var sheep       = Entity.Create();

            sheep.Tag("npc");

            var width       = RPGWorld.UnitToPixel * 1.3f;
            var height      = RPGWorld.UnitToPixel * 0.7f;
            
            var transform   = sheep.AddComponent<Transform>();

            var collider            = sheep.AddComponent<Collider>();
            collider.MakeDynamicBox(width, height);
            collider.Category       = CollisionCategories.Entitites;
            collider.CollidesWith   = CollisionCategories.World;
            
            var controller           = sheep.AddComponent<NPCController>();
            controller.Handler       = NPCHandlerFactory.Instance.Create("CritterHandler");
            
            var rendrer             = sheep.AddComponent<SpriteRenderer>();
            rendrer.Flags           = RenderFlags.Anchored | RenderFlags.AutomaticDepth;
            rendrer.Sprite.Layer    = Layers.Middle;
            rendrer.Sprite.Texture  = Engine.Instance.Content.Load<Texture2D>("sheep");
            rendrer.Sprite.Source   = new Rectangle(0, 0, 32, 32);

            return sheep;
        }

        public static void Create()
        {
            var builder = EntityBuilder.Instance;

            builder.RegisterPrefab("player", PlayerPrefab);
            builder.RegisterPrefab("sheep", SheepPrefab);
        }
    }
}
