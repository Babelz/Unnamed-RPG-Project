using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vRPGEngine.Graphics
{
    public interface IRenderable
    {
        #region Properties
        Vector2 Position
        {
            get;
        }
        Vector2 Center
        {
            get;
        }
        Vector2 Scale
        {
            get;
        }
        Vector2 Size
        {
            get;
        }

        int Layer
        {
            get;
            set;
        }
        Point Cell
        {
            get;
            set;
        }
        int Index
        {
            get;
            set;
        }

        bool Visible
        {
            get;
        }
        #endregion

        void Present();
    }
}
