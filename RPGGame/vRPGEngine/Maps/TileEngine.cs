using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vRPGEngine.Maps
{
    public static class TileEngine
    {
        #region Properties
        public static int TileWidth
        {
            get;
            private set;
        }
        public static int TileHeight
        {
            get;
            private set;
        }
        public static int MapWidth
        {
            get;
            private set;
        }
        public static int MapHeight
        {
            get;
            private set;
        }
        #endregion

        #region Events
        public static event EventHandler PropertiesChanged;
        #endregion

        public static void ChangeProperties(int tileWidth, int tileHeight, int mapWidth, int mapHeight)
        {
            TileWidth   = tileWidth;
            TileHeight  = tileHeight;

            MapWidth    = mapWidth;
            MapHeight   = mapHeight;

            PropertiesChanged?.Invoke(null, EventArgs.Empty);
        }
    }
}
