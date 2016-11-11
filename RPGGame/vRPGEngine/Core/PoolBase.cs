using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace vRPGEngine.Core
{
    public abstract class PoolBase<T> where T : class
    {
        #region Fields
        private readonly Func<T> newFunction;
        #endregion
        
        public PoolBase(Func<T> newFunction)
        {
            Debug.Assert(newFunction != null);

            this.newFunction = newFunction;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected T CreateSingle()
        {
            return newFunction();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected T[] CreateArray(int length)
        {
            Debug.Assert(length != 0);

            var array = new T[length];

            for (var i = 0; i < array.Length; i++) array[i] = CreateSingle();

            return array;
        }
    }
}
