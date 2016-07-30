using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vRPGEngine
{
    public abstract class Singleton<T> where T : class
    {
        #region Fields
        private static readonly T instance;
        #endregion

        #region Properties
        public static T Instance
        {
            get
            {
                return instance;
            }
        }
        #endregion

        static Singleton()
        {
            instance = Activator.CreateInstance(typeof(T), true) as T;
        }

        protected Singleton()
        {
        }
    }
}
