using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vRPGEngine.ECS.Components;

namespace vRPGEngine.Combat
{
    public sealed class CombatManager : SystemManager<CombatManager>
    {
        #region Fields
        private readonly List<ICharacterController> hostiles;
        #endregion

        private CombatManager()
            : base()
        {
        }
    }
}
