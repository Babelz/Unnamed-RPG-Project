using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vRPGEngine.Input
{
    public interface IBindingCollection<T>
    {
        IEnumerable<T> Bindings();
    }

    public interface IKeyboardBindingCollection : IBindingCollection<KeyboardBinding>
    {
    }

    public interface IMouseBindingCollection : IBindingCollection<MouseBinding>
    {
        IEnumerable<MouseInputEventHandler> ScrollBindings();
        IEnumerable<MouseInputEventHandler> MoveBindings();
    }
}
