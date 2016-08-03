using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiledSharp;
using vRPGEngine.ECS;

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

        public IEnumerable<Entity> Entitites()
        {
            return entitites;
        }

        public void Load(string name)
        {
            data = vRPGEngine.Instance.Content.Load<TmxMap>(name);

            // Load map.

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
