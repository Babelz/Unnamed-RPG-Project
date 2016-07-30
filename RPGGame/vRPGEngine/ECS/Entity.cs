using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vRPGEngine.ECS
{
    public sealed class Entity : IRegisterEntry
    {
        #region Properties
        public int Location
        {
            get;
            set;
        }
        #endregion

        public Entity()
        {
        }


    }
}
