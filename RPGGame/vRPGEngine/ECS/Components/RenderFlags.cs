using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vRPGEngine.ECS.Components
{
    [Flags()]
    public enum RenderFlags : int
    {
        None = 0,
        Anchored = 1,
        AutomaticDepth = 2,
        All = Anchored | AutomaticDepth
    }
}
