using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiledSharp;
using vRPGEngine.ECS;

namespace vRPGEngine.Map
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
            data = new TmxMap(name);


        }

        public void Unload()
        {
        }
    }
}
