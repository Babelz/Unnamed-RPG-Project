using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vRPGEngine.ECS.Components;

namespace vRPGEngine.ECS
{
    public sealed class DataDictionaryManager : ComponentManager<DataDictionary>
    {
        private DataDictionaryManager()
            : base()
        {
        }
    }
}
