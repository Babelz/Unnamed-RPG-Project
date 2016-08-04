using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vRPGEngine.ECS.Components;

namespace vRPGEngine.ECS.Handlers
{
    public interface IComponentUpdateHanlder<T> where T : class, IComponent, new()
    {
        void Update(ComponentManager<T> manager, IEnumerable<T> components, GameTime gameTime);
    }
}
