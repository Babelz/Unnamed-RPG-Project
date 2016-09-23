using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vRPGEngine.Interfaces
{
    public interface IGenericCloneable<T> where T : class
    {
        T Clone();
    }
}
