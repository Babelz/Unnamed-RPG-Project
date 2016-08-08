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
        #region Constants
        private const int MapLayer = 0;
        #endregion

        #region Fields
        private List<Entity> entitites;

        private TmxMap data;
        #endregion

        private TileMapManager()
            : base()
        {
            entitites = new List<Entity>();
        }

        private Entity ImageLayer(string name, string image, float x, float y, float opacity, bool visible)
        {
            var layer               = EntityBuilder.Instance.Create("empty");
            layer.Tags              = name;

            var renderer            = layer.AddComponent<SpriteRenderer>();
            renderer.Sprite.Texture = vRPGEngine.Instance.Content.Load<Texture2D>(image);
            renderer.Sprite.Color   = new Color(renderer.Sprite.Color, opacity);
            renderer.Sprite.Visible = visible;
            renderer.Sprite.Layer   = MapLayer;

            var transform           = layer.FirstComponentOfType<Transform>();
            transform.Position      = new Vector2(x, y);
            
            return layer;
        }

        private Entity Layer(string name)
        {
            var layer   = Entity.Create();
            layer.Tags  = name;

            return layer;
        }

        private Entity Tile(float x, float y, Rectangle src, Texture2D tex, float opacity, bool visible)
        {
            var tile                 = Entity.Create();

            var renderer              = tile.AddComponent<SpriteRenderer>();
            renderer.Sprite.Texture   = tex;
            renderer.Sprite.Position  = new Vector2(x, y);
            renderer.Sprite.Size = new Vector2(TileEngine.TileWidth, TileEngine.TileHeight);
            renderer.Sprite.Color     = new Color(renderer.Sprite.Color, opacity);
            renderer.Sprite.Visible   = visible;
            renderer.Sprite.Source    = src;
            renderer.Sprite.Layer     = MapLayer;
            
            return tile;
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
                var opacity     = layer.Opacity;
                var visible     = layer.Visible;

                var tileLayer   = Layer(layer.Name);
                
                var tilesCount  = 0;

                foreach (var tile in layer.Tiles)
                {
                    var gid = tile.Gid;

                    if (gid == 0) continue;

                    var tileFrame   = gid - 1;
                    var tileset     = data.Tilesets.FirstOrDefault(t => t.FirstGid <= gid);
                    var texname     = tileset.Image.Source.Substring(0, tileset.Image.Source.LastIndexOf("."));
                    var tex         = vRPGEngine.Instance.Content.Load<Texture2D>(texname);
                    var row         = (int)Math.Floor((double)tileFrame / (double)(tex.Width / tileWidth));
                    var column      = tileFrame % (tex.Width / tileWidth);
                    var src         = new Rectangle(tileWidth * column, tileHeight * row, tileWidth, tileHeight);
                    var x           = (float)tile.X * tileWidth;
                    var y           = (float)tile.Y * tileHeight;
                    
                    tileLayer.AddChildren(Tile(x, y, src, tex, (float)opacity, visible));

                    tilesCount++;
                }
            }

            // Load entitites.
            foreach (var layer in data.ObjectGroups)
            {
                var opacity     = layer.Opacity;
                var visible     = layer.Visible;

                var objectLayer = Layer(layer.Name);

                foreach (var entity in layer.Objects)
                {
                    var type        = entity.Type;
                    var x           = entity.X;
                    var y           = entity.Y;
                    var width       = entity.Width;
                    var height      = entity.Height;
                    var rotation    = entity.Rotation;

                    // TODO: spawn.
                }
            }

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
