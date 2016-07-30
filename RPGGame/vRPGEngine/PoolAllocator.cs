using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vRPGEngine
{
    public sealed class PoolAllocator<T> where T : class
    {
        #region Fields
        private readonly Func<T> alloc;

        private Stack<T> released;
        private T[] pool;

        private int ptr;
        #endregion

        #region Properties
        /// <summary>
        /// Total size of the pool.
        /// </summary>
        public int Size
        {
            get
            {
                return pool.Length;
            }
        }
        /// <summary>
        /// How many elements are released.
        /// </summary>
        public int Released
        {
            get
            {
                return released.Count;
            }
        }
        #endregion

        public PoolAllocator(int initialPoolSize, Func<T> alloc)
        {
            Debug.Assert(initialPoolSize != 0);
            Debug.Assert(alloc != null);

            this.alloc = alloc;

            pool = new T[initialPoolSize];

            for (var i = 0; i < pool.Length; i++) pool[i] = alloc();
        }
        public PoolAllocator(Func<T> alloc)
            : this(8, alloc)
        {
        }

        private void ReallocatePool()
        {
            // Create new pool.
            var oldLength   = pool.Length;
            var newPool     = new T[oldLength * 2];

            // Copy old pool contents to new.
            Array.Copy(pool, newPool, newPool.Length);

            for (var i = oldLength; i < newPool.Length; i++) newPool[i] = alloc();

            pool = newPool;
        }

        public T Allocate()
        {
            // Allocate from released.
            if (released.Count != 0)    return released.Pop();
            // Allocate from pool.
            if (ptr + 1 < pool.Length)  return pool[ptr++];

            // Realloc and alloc from pool.
            ReallocatePool();

            return pool[ptr++];
        }
        public void Deallocate(T element)
        {
            Debug.Assert(pool.Contains(element));

            released.Push(element);
        }
    }
}
