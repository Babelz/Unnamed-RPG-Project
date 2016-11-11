using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace vRPGEngine.Core
{
    public interface IFreeListEntry
    {
        #region Properties
        int Location
        {
            get;
            set;
        }
        #endregion
    }

    public sealed class FreeList<T> : PoolBase<T> where T : class, IFreeListEntry
    {
        #region Fields
        private Stack<int> released;

        private T[] pool;
        private int ptr;
        #endregion

        #region Properties
        public int Size
        {
            get
            {
                return pool.Length;
            }
        }
        #endregion

        public FreeList(int initialCapacity, Func<T> objectAllocator)
            : base(objectAllocator)
        {
            Debug.Assert(initialCapacity != 0);

            released    = new Stack<int>();
            pool        = CreateArray(initialCapacity);

            for (var i = 0; i < pool.Length; i++) pool[i].Location = i;
        }
        public FreeList(Func<T> objectAllocator)
            : this(8, objectAllocator)
        {
        }

        private void ResizePool()
        {
            var oldLength   = pool.Length;
            var newPool     = new T[oldLength * 2];

            // Copy old pool contents to new.
            Array.Copy(pool, newPool, pool.Length);

            for (var i = oldLength; i < newPool.Length; i++)
            {
                // Create, store location.
                var element         = CreateSingle();
                element.Location    = i;

                // Store reference.
                newPool[i] = element;
            }

            pool = newPool;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T Create()
        {
            if (released.Count != 0)    return pool[released.Pop()];
            
            if (ptr + 1 < pool.Length)  return pool[ptr++];

            ResizePool();

            return pool[ptr++];
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Release(T entry)
        {
            Debug.Assert(entry != null);

            if (entry.Location >= pool.Length) throw new vRPGEngineException("entry not located in this allocator");

            released.Push(entry.Location);
        }
    }
}
