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

            var rendrer   = player.AddComponent<SpriteRenderer>();
            rendrer.Flags = RenderFlags.Anchored | RenderFlags.AutomaticDepth;

            rendrer.Sprite.Layer = Layers.Middle;
            rendrer.Sprite.ScaleTo(new Vector2(32.0f), rendrer.Sprite.Texture);

            var collider            = player.AddComponent<BoxCollider>();
            collider.MakeDynamic(32.0f, 32.0f);
            collider.Category       = CollisionCategories.Entitites;
            collider.CollidesWith   = CollisionCategories.World;

            var behaviour = player.AddComponent<Behaviour>();

            var view                 = new View(vRPGEngine.Engine.Instance.GraphicsDevice.Viewport);
            var zoomStep             = 0.1f;

            // TODO: load from game data.
            var data                  = SpecializationDatabase.Instance.Elements().First(s => s.Name.ToLower() == "warrior");
            var controller            = player.AddComponent<PlayerCharacterController>();
            var attributes            = new AttributesData()
            {
                Level = 10
            };

            var equipments            = new EquipmentContainer()
            {
                MainHand = WeaponDatabase.Instance.Elements().First()
            };

            var meleeDamageController = new MeleeDamageController();
            var statuses              = new Statuses();
            var specialization        = new Warrior(attributes, equipments, statuses, meleeDamageController);

            meleeDamageController.Initialize(equipments, specialization);
            statuses.Initialize(specialization);
            controller.Initialize(specialization, attributes, equipments, meleeDamageController, statuses);

            behaviour.Behave = new Action<GameTime>((gameTime) =>
            {
                collider.LinearVelocity = Vector2.Zero;
                view.Position           = transform.Position;

                view.FocuCenter();
            });

            // Init input.
            var kip = InputManager.Instance.GetProvider<KeyboardInputProvider>();
            var velo = 2.0f;

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

            kip.Bind("auto_attack", Keys.D1, KeyTrigger.Pressed, () =>
            {
                controller.Spells.FirstOrDefault(s => s.Spell.Name == "Auto attack").Use(player);
            });
            kip.Bind("battle_shout", Keys.D2, KeyTrigger.Pressed, () =>
            {
                controller.Spells.FirstOrDefault(s => s.Spell.Name == "Battle shout").Use(player);
            });

            kip.Bind("zoom_in", Keys.Q, KeyTrigger.Pressed, () => view.Zoom += zoomStep);
            kip.Bind("zoom_out", Keys.E, KeyTrigger.Pressed, () => view.Zoom -= zoomStep);

            var mip = InputManager.Instance.GetProvider<MouseInputProvider>();

            mip.Bind("click", MouseButton.LeftButton, MouseTrigger.Pressed, (ms) =>
            {
                var position = view.ScreenToView(ms.Position);
                var radius   = 2.5f;

                controller.TargetFinder.FindTarget(position, radius);
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

            var collider            = sheep.AddComponent<BoxCollider>();
            collider.MakeDynamic(width, height);
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
