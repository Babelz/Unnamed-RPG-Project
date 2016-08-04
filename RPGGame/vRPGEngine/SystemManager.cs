using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace vRPGEngine
{
    public interface ISystemManager
    {
        #region Properties
        Guid ID
        {
            get;
        }
        bool Active
        {
            get;
        }
        #endregion

        void Activate();
        void Suspend();

        void Update(GameTime gameTime);
    }

    public abstract class SystemManager<T> : Singleton<T>, ISystemManager where T : class
    {
        #region Properties
        public Guid ID
        {
            get;
            private set;
        }
        public bool Active
        {
            get;
            private set;
        }
        #endregion
        
        protected SystemManager() 
            : base()
        {
            ID = Guid.NewGuid();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected virtual void OnActivate()
        {
            SystemManagers.Instance.Register(this);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected virtual void OnSuspend()
        {
            SystemManagers.Instance.Unregister(this);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected virtual void OnUpdate(GameTime gameTime)
        {
        }

        public void Activate()
        {
            if (Active) return;

            OnActivate();

            Active = true; 
        }
        public void Suspend()
        {
            if (!Active) return;

            OnSuspend();

            Active = false;
        }

        public void Update(GameTime gameTime)
        {
            if (!Active) return;

            OnUpdate(gameTime);
        }
    }
}
