using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vRPGEngine
{
    public sealed class SampleList<T> where T : struct
    {
        #region Fields
        private readonly T[] samples;

        private readonly int maxSamples;

        private int ptr;
        #endregion

        #region Properties
        public IEnumerable<T> Values
        {
            get
            {
                for (var i = 0; i < maxSamples; i++) yield return samples[i];
            }
        }

        public T this[int index]
        {
            get
            {
                return samples[index];
            }
        }

        public int MaxSamples
        {
            get
            {
                return maxSamples;
            }
        }
        public int SamplesCount
        {
            get
            {
                return ptr;
            }
        }
        #endregion

        public SampleList(int maxSamples)
        {
            this.maxSamples = maxSamples;

            samples         = new T[maxSamples];
        }

        public T First()
        {
            return samples[0];
        }
        public void Add(T sample)
        {
            samples[ptr++] = sample;

            if (ptr == maxSamples) ptr = 0;
        }
    }
}
