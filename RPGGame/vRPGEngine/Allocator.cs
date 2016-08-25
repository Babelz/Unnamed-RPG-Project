using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace vRPGEngine
{
    public abstract class Allocator<T> where T : class
    {
        #region Fields
        private readonly Func<T> alloc;
        #endregion
        
        public Allocator(Func<T> alloc)
        {
            Debug.Assert(alloc != null);

            this.alloc = alloc;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected T AllocateSingle()
        {
            return alloc();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected T[] AllocateArray(int length)
        {
            Debug.Assert(length != 0);

            var array = new T[length];

            for (var i = 0; i < array.Length; i++) array[i] = AllocateSingle();

            return array;
        }
    }
}
