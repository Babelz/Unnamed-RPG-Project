using FarseerPhysics.Dynamics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPGGame
{
    public static class CollisionCategories
    {
        #region Fields
        public static readonly Category Entitites   = Category.Cat1;
        public static readonly Category World       = Category.Cat2 | Category.Cat3;
        #endregion
    }
}
