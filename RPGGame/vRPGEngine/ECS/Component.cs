using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vRPGEngine.ECS
{
    public abstract class Component
    {
        #region Fields
        private Entity owner;
        #endregion
        
        public Component()
        {
        }
    }
}
