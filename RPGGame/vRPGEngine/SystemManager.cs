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
        int ID
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

    public abstract class SystemManager<T> : Singleton<T> where T : class, new()
    {
        #region Fields
        /// <summary>
        /// Used to generate identifiers for the system managers.
        /// </summary>
        private static int idc;

        private readonly int id;

        private bool active;
        #endregion

        #region Properties
        public int ID
        {
            get
            {
                return id;
            }
        }
        public bool Active
        {
            get
            {
                return active;
            }
        }
        #endregion

        static SystemManager()
        {
            idc = 0;
        }

        public SystemManager() 
            : base()
        {
            id = idc++;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected virtual void OnActivate()
        {
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected virtual void OnSuspend()
        {
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected virtual void OnUpdate(GameTime gameTime)
        {
        }
        
        void Activate()
        {
            if (Active) return;

            OnActivate();

            active = true; 
        }
        void Suspend()
        {
            if (!Active) return;

            OnSuspend();

            active = false;
        }

        public void Update(GameTime gameTime)
        {
            if (!Active) return;

            OnUpdate(gameTime);
        }
    }
}
