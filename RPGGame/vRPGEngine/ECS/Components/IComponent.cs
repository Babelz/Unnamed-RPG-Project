using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vRPGEngine.ECS.Components
{
    public interface IComponent : IRegisterEntry
    {
        #region Properties
        Entity Owner
        {   
            get;
            set;
        }
        #endregion
        
        void Destroy();
    }
}
