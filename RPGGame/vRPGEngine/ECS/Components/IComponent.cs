using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vRPGEngine.Core;

namespace vRPGEngine.ECS.Components
{
    public interface IComponent : IFreeListEntry
    {
        #region Properties
        Entity Owner
        {   
            get;
            set;
        }
        #endregion
        
        #region Events
        event ComponentEventHandler Destroyed;
        #endregion

        void Destroy();
    }

    public delegate void ComponentEventHandler(IComponent instance);
}
