using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vRPGEngine
{
    public abstract class SystemManager<T> : Singleton<T> where T : class, new()
    {
        #region Fields
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
    }
}
