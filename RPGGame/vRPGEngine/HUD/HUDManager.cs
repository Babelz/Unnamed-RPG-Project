using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vRPGEngine.Attributes;
using vRPGEngine.Attributes.Spells;
using vRPGEngine.Core;
using vRPGEngine.ECS;
using vRPGEngine.ECS.Components;
using vRPGEngine.Graphics;
using vRPGEngine.Handlers.Spells;
using vRPGEngine.HUD.Controls;
using vRPGEngine.HUD.Elements;
using vRPGEngine.HUD.Interfaces;

namespace vRPGEngine.HUD
{
    public sealed class HUDManager : SystemManager<HUDManager>
    {
        #region Fields
        private HUDConstructor constructor;
        #endregion

        #region Properties
        public View View
        {
            get
            {
                return constructor?.View;
            }
        }
        public IContentControl Root
        {
            get
            {
                return constructor?.Root;
            }
        }
        public IEnumerable<HUDSubsystem> Subsystems
        {
            get
            {
                return constructor?.Subsystems;
            }
        }
        #endregion

        private HUDManager()
            : base()
        {
        }

        protected override void OnSuspend()
        {
            if (constructor != null) constructor.Deconstruct();
        }

        public void ConstructFrom(HUDConstructor constructor)
        {
            if (this.constructor != null) this.constructor.Deconstruct();

            this.constructor = constructor;

            this.constructor.Construct();

            var valid = this.constructor.Root != null && this.constructor.View != null;

            if (this.constructor.Root == null) Logger.Instance.LogError("root can't be null!");
            if (this.constructor.View == null) Logger.Instance.LogError("view can't be null!");

            if (!valid)
            {
                this.constructor.Deconstruct();

                this.constructor = null;
            }
        }
        public void Deconstruct()
        {
            if (constructor == null) return;

            constructor.Deconstruct();

            constructor = null;
        }
    }
}
