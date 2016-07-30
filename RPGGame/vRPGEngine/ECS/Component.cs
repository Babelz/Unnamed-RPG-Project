using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vRPGEngine.ECS
{
    public abstract class Component : IRegisterEntry
    {
        #region Fields
        private Entity owner;
        #endregion

        #region Properties
        public int Location
        {
            get;
            set;
        }
        #endregion

        public Component()
        {
        }
    }
}
