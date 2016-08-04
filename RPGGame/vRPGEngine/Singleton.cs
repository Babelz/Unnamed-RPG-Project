using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vRPGEngine
{
    /// <summary>
    /// Singleton base class. To use this inherit from this class
    /// and set the T as the base class type. Set the default
    /// constructor private when using this base class.
    /// </summary>
    /// <typeparam name="T">type of the singleton instance</typeparam>
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
