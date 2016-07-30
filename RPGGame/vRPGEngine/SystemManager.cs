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
        string Name
        {
            get;
        }
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

    public abstract class SystemManager<T> : Singleton<T> where T : class
    {
        #region Fields
        private bool active;

        /// <summary>
        /// Used to generate identifiers for the system managers.
        /// </summary>
        private static int idc;

        private readonly string name;
        private readonly int id;
        #endregion

        #region Properties
        public string Name
        {
            get
            {
                return name;
            }
        }
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

        protected SystemManager(string name) 
            : base()
        {
            Debug.Assert(!string.IsNullOrEmpty(name));

            this.name = name;

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
