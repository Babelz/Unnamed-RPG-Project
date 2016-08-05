using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace vRPGData
{
    [Serializable()]
    public sealed class DataCollection<T>
    {
        public T[] data;
    }
}
