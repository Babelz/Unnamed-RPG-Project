using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vRPGEngine.Graphics;
using vRPGEngine.HUD.Interfaces;

namespace vRPGEngine.HUD
{
    /// <summary>
    /// Interface that can be used to wrap the creation
    /// and destruction of an HUD or GUI element.
    /// </summary>
    public abstract class HUDConstructor
    {
        #region Properties
        /// <summary>
        /// The root of this HUD. 
        /// </summary>
        public IContentControl Root
        {
            get;
            protected set;
        }
        public View View
        {
            get;
            protected set;
        }
        public IEnumerable<HUDSubsystem> Subsystems
        {
            get;
            protected set;
        }
        #endregion

        public HUDConstructor()
        {
        }
        
        /// <summary>
        /// Construct HUD and add it to the manager.
        /// </summary>
        public abstract void Construct();

        /// <summary>
        /// Deconstruct this HUD and remove it from the manager.
        /// </summary>
        public virtual void Deconstruct()
        {
        }
    }
}
