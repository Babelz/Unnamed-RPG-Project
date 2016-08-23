using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        #region Events
        public event CombatManagerEventHandler HostileRegistered;
        public event CombatManagerEventHandler HostileUnregistered;
        public event CombatManagerEventHandler HostilesEmpty;
        #endregion
        
        private CombatManager()
            : base()
        {
            hostiles = new List<ICharacterController>();
        }

        public void RegisterHostile(ICharacterController hostile)
        {
            Debug.Assert(hostile != null);

            hostiles.Add(hostile);

            HostileRegistered?.Invoke();
        }

        public void UnregisterHostile(ICharacterController hostile)
        {
            Debug.Assert(hostile != null);

            hostiles.Remove(hostile);

            HostileUnregistered?.Invoke();

            if (!HasHostiles()) HostilesEmpty?.Invoke();
        }

        public bool HasHostiles()
        {
            return hostiles.Count != 0;
        }

        public delegate void CombatManagerEventHandler();
    }
}
