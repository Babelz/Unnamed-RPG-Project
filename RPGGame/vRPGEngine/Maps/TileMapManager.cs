using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Contacts;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiledSharp;
using vRPGEngine.Core;
using vRPGEngine.Databases;
using vRPGEngine.ECS;
using vRPGEngine.ECS.Components;
using vRPGEngine.Graphics;

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

        private string TryGetProperty(PropertyDict dict, string name, string defaultValue)
        {
            if (dict == null || dict.Dict == null) return defaultValue;
            
            var value = string.Empty;

            if (dict.Dict.TryGetValue(name, out value)) return value;

            return defaultValue;
        }


        private Entity ImageLayer(string name, string image, float x, float y, float opacity, bool visible, int location)
        {
            var layer               = EntityBuilder.Instance.Create("empty");
            layer.Tag(name);

            var renderer            = layer.AddComponent<SpriteRenderer>();
            renderer.Sprite.Layer   = location;
            renderer.Sprite.Texture = Engine.Instance.Content.Load<Texture2D>(image);
            renderer.Sprite.Color   = new Color(renderer.Sprite.Color, opacity);
            renderer.Sprite.Visible = visible;
            renderer.Sprite.Layer   = location;
            renderer.Flags          = RenderFlags.None;
            
            var transform           = layer.FirstComponentOfType<Transform>();
            transform.Position      = new Vector2(x, y);
            
            return layer;
        }

        private Entity Layer(string name)
        {
            var layer   = Entity.Create();

            layer.Tag(name);

            return layer;
        }

        private Entity Tile(float x, float y, Rectangle src, Texture2D tex, float opacity, bool visible, int location, float depth)
        {
            var tile                  = Entity.Create();

            var renderer              = tile.AddComponent<SpriteRenderer>();
            renderer.Sprite.Texture   = tex;
            renderer.Sprite.Position  = new Vector2(x, y);
            renderer.Sprite.Color     = new Color(renderer.Sprite.Color, opacity);
            renderer.Sprite.Visible   = visible;
            renderer.Sprite.Source    = src;
            renderer.Sprite.Layer     = location;
            renderer.Flags            = RenderFlags.None;
            renderer.Sprite.Depth     = depth;

            return tile;
        }

        private Entity Wall(float x, float y, float width, float height, float rotation)
        {
            var wall  = Entity.Create();

            wall.Tag("wall");

            var collider          = wall.AddComponent<Collider>();
            collider.MakeStaticBox(width, height, x, y);
            collider.Category     = Category.Cat2;
            collider.CollidesWith = Category.Cat1;

            return wall;
        }

        private Entity ComplexWall(float x, float y, Vector2[] points)
        {
            var wall    = Entity.Create();

            wall.Tag("wall");

            var collider          = wall.AddComponent<Collider>();
            collider.MakeStaticPolygon(x, y, points);
            collider.Category     = Category.Cat2;
            collider.CollidesWith = Category.Cat1;

            return wall;
        }

        private Entity SpawnArea(int id, int maxNPCs, int minLevel, int maxLevel, int spawnTime, float x, float y, float width, float height, float maxDist)
        {
            var area = EntityBuilder.Instance.Create("spawn area");

            var controller = area.FirstComponentOfType<SpawnArea>();

            var data = NPCDatabase.Instance.Elements().First(n => n.ID == id);

            if (data == null)
            {
                Logger.Instance.LogFunctionWarning(string.Format("npc data with id {0} was not found", id));

                return area;
            }

            controller.Initialize(data, new Vector2(x, y), new Vector2(width, height), minLevel, maxLevel, maxNPCs, spawnTime, maxDist);

            return area;
        }

        public IEnumerable<Entity> Entitites()
        {
            return entitites;
        }

        public void Load(string name)
        {
            data = Engine.Instance.Content.Load<TmxMap>(name);

            // Set tile-engine params.
            var tileWidth = data.TileWidth;
            var tileHeight = data.TileHeight;
            var mapWidth = data.Width;
            var mapHeight = data.Height;

            TileEngine.ChangeProperties(tileWidth, tileHeight, mapWidth, mapHeight);

            // Load map.
            foreach (var layer in data.ImageLayers)
            {

                var value    = string.Empty;
                var location = int.Parse(TryGetProperty(layer.Properties, "location", MapLayer.ToString()));

                entitites.Add(ImageLayer(layer.Name,
                                         layer.Image?.Source,
                                         (float)layer.OffsetX,
                                         (float)layer.OffsetY,
                                         (float)layer.Opacity,
                                         layer.Visible,
                                         location));
            }
            
            foreach (var layer in data.Layers)
            {
                var opacity     = layer.Opacity;
                var visible     = layer.Visible;

                var tileLayer   = Layer(layer.Name);
                
                var tilesCount  = 0;

                var value       = string.Empty;
                var location    = int.Parse(TryGetProperty(layer.Properties, "location", MapLayer.ToString()));
                
                foreach (var tile in layer.Tiles)
                {
                    var gid = tile.Gid;

                    if (gid == 0) continue;

                    var tileFrame   = gid - 1;
                    var tileset     = data.Tilesets.FirstOrDefault(t => t.FirstGid <= gid);
                    var texname     = tileset.Image.Source.Substring(0, tileset.Image.Source.LastIndexOf("."));
                    var tex         = Engine.Instance.Content.Load<Texture2D>(texname);
                    var row         = (int)Math.Floor(tileFrame / (double)(tex.Width / tileWidth));
                    var column      = tileFrame % (tex.Width / tileWidth);
                    var src         = new Rectangle(tileWidth * column, tileHeight * row, tileWidth, tileHeight);
                    var x           = (float)tile.X * tileWidth;
                    var y           = (float)tile.Y * tileHeight;
                    var depth       = Math.Abs((y + tileHeight)) / (mapHeight * tileHeight);
                    
                    tileLayer.AddChildren(Tile(x, y, src, tex, (float)opacity, visible, location, depth));

                    tilesCount++;
                }

                entitites.Add(tileLayer);
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
                    var x           = (float)entity.X;
                    var y           = (float)entity.Y;
                    var width       = (float)entity.Width;
                    var height      = (float)entity.Height;
                    var rotation    = (float)entity.Rotation;

                    if (entity.Name == "wall")
                    {
                        objectLayer.AddChildren(Wall(x, y, width, height, rotation));
                    }
                    else if (entity.Name == "walls")
                    {
                        objectLayer.AddChildren(ComplexWall(x, y, entity.Points.Select(p => new Vector2((float)p.X, (float)p.Y)).ToArray()));
                    }
                    else if (entity.Name == "spawn area")
                    {
                        var id          = int.Parse(TryGetProperty(entity.Properties, "id", "0"));
                        var maxDist     = float.Parse(TryGetProperty(entity.Properties, "maxdist", ECS.Components.SpawnArea.DefaultMaxDist.ToString()));
                        var maxLevel    = int.Parse(TryGetProperty(entity.Properties, "maxlvl", "0"));
                        var minLevel    = int.Parse(TryGetProperty(entity.Properties, "minlvl", "0"));
                        var spawnTime   = int.Parse(TryGetProperty(entity.Properties, "spawntime", ECS.Components.SpawnArea.DefaultSpawnTime.ToString()));
                        var maxNPCs     = int.Parse(TryGetProperty(entity.Properties, "maxnpcs", ECS.Components.SpawnArea.DefaultMaxNPCs.ToString()));
                        
                        objectLayer.AddChildren(SpawnArea(id, maxNPCs, minLevel, maxLevel, spawnTime, x, y, width, height, maxDist));
                    }
                }

                entitites.Add(objectLayer);
            }

            // Load state from saved game.
            
            // Done.
        }

        public void Unload()
        {
            // Unload map.

            // Save state to saved game.

            // Unload entitites.
            for (var i = 0; i < entitites.Count; i++) entitites[i].Destroy();

            // Done.
        }
    }
}
