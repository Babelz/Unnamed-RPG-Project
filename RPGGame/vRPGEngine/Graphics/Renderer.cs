using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vRPGEngine.Graphics
{
    public sealed class Renderer : Singleton<Renderer>
    {
        #region Fields
        private readonly List<Layer> layers;
        #endregion

        #region Properties
        public IEnumerable<Layer> Layers
        {
            get
            {
                return layers;
            }
        }
        #endregion
    }
}
