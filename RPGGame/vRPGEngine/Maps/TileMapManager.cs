using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiledSharp;
using vRPGEngine.ECS;
using vRPGEngine.ECS.Components;

namespace vRPGEngine.Maps
{
    public sealed class TileMapManager : Singleton<TileMapManager>
    {
        #region Fields
        private List<Entity> entitites;

        private TmxMap data;
        #endregion

        private TileMapManager()
            : base()
        {
        }

        private Entity ImageLayer(string name, string image, float x, float y, float opacity, bool visible)
        {
            var layer               = EntityBuilder.Instance.Create("empty");
            layer.Tags              = name;

            var renderer            = layer.AddComponent<SpriteRenderer>();
            renderer.Sprite.Texture = vRPGEngine.Instance.Content.Load<Texture2D>(image);
            renderer.Sprite.Color   = new Color(renderer.Sprite.Color, opacity);
            renderer.Sprite.Visible = visible;

            var transform           = layer.FirstComponentOfType<Transform>();
            transform.Position      = new Vector2(x, y);
            
            return layer;
        }

        private Entity TileLayer(string name)
        {
            var layer   = Entity.Create();
            layer.Tags  = name;

            return layer;
        }

        public IEnumerable<Entity> Entitites()
        {
            return entitites;
        }

        public void Load(string name)
        {
            data = vRPGEngine.Instance.Content.Load<TmxMap>(name);

            // Set tile-engine params.
            var tileWidth = data.TileWidth;
            var tileHeight = data.TileHeight;
            var mapWidth = data.Width;
            var mapHeight = data.Height;

            TileEngine.ChangeProperties(tileWidth, tileHeight, mapWidth, mapHeight);
            
            // Load map.
            foreach (var layer in data.ImageLayers)
                entitites.Add(ImageLayer(layer.Name, 
                                         layer.Image?.Source, 
                                         (float)layer.OffsetX,
                                         (float)layer.OffsetY,
                                         (float)layer.Opacity, 
                                         layer.Visible));

            foreach (var layer in data.Layers)
            {
                var opacity = layer.Opacity;
                var visible = layer.Visible;

                var entity = TileLayer(layer.Name);

                foreach (var tile in layer.Tiles)
                {
                    var gid = tile.Gid;

                    if (gid == 0) continue;

                    var tileFrame = gid - 1;
                    var column = 0;
                    var row = 0;

                    var src = new Rectangle(tileWidth * column, tileHeight * row, tileWidth, tileHeight);
                        
                }
            }

            // Load entitites.

            // Load state from saved game.
            
            // Done.
        }
        
        public void Unload()
        {
            // Unload map.

            // Save state to saved game.

            // Unload entitites.

            // Done.
        }
    }
}
