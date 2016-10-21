using RPGGame.GUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vRPGEngine;
using vRPGEngine.ECS;
using vRPGEngine.Graphics;
using vRPGEngine.HUD;
using vRPGEngine.Maps;
using vRPGEngine.Scenes;

namespace RPGGame.Scenes
{
    public sealed class GameplayTestingScene : Scene
    {
        public GameplayTestingScene()
            : base()
        {
            TileEngine.PropertiesChanged += TileEngine_PropertiesChanged;
        }

        #region Event handlers
        private void TileEngine_PropertiesChanged(object sender, EventArgs e)
        {
            var layerWidth  = TileEngine.MapWidth * TileEngine.TileWidth;
            var layerHeight = TileEngine.MapHeight * TileEngine.TileHeight;
            var cellWidth   = layerWidth / 16;
            var cellHeight  = layerHeight / 16;

            Renderer.Instance.SetPresentationParameters(layerWidth, layerHeight, cellWidth, cellHeight);
            Layers.Create();
        }
        #endregion

        public override void Initialize()
        {
            Renderer.Instance.DynamicPadding = true;

            Prefabs.Create();
            
            TileMapManager.Instance.Load("demo");

            var player = EntityBuilder.Instance.Create("player");
            
            HUDManager.Instance.ConstructFrom(new GameplayHUDConstructor());
        }
        public override void Deinitialize()
        {
            TileMapManager.Instance.Unload();

            EntityManager.Instance.Clear();
        }
    }
}
