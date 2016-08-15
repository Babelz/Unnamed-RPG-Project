using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vRPGEngine.ECS;
using vRPGEngine.Graphics;
using vRPGEngine.Maps;
using vRPGEngine.Scenes;

namespace RPGGame.Scenes
{
    public sealed class GameplayTestingScene : Scene
    {
        public GameplayTestingScene()
            : base()
        {
        }

        public override void Initialize()
        {
            Renderer.Instance.SetPresentationParameters(3200, 3200, 160, 160, 3, 3);
            Renderer.Instance.DynamicPadding = true;

            Prefabs.Register();
            Layers.Create();

            TileMapManager.Instance.Load("test bed map");

            var player = EntityBuilder.Instance.Create("player");
        }
        public override void Deinitialize()
        {
            TileMapManager.Instance.Unload();

            EntityManager.Instance.Clear();
        }
    }
}
