using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vRPGEngine.ECS.Components
{
    public sealed class Transform : Component<Transform>
    {
        #region Properties
        public Vector2 Position
        {
            get;
            set;
        }
        public Vector2 Size
        {
            get;
            set;
        }
        public Vector2 Scale
        {
            get;
            set;
        }
        #endregion

        public Transform()
            : base()
        {
        }
    }
}
