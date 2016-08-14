using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vRPGEngine.HUD
{
    public interface IContentControl
    {
        #region Properties
        IEnumerable<Control> Children
        {
            get;
        }
        #endregion

        void Add(Control child);
        void Remove(Control child);

        IEnumerable<Control> Query(Func<Control, bool> predicate);

        void Update(GameTime gameTime);
    }
}
