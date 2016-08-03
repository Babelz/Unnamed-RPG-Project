using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vRPGEngine.Data
{
    [Serializable()]
    public sealed class Material : Item
    {
        #region Properties
        public Categories Category
        {
            get;
            set;
        }
        #endregion

        public Material()
            : base()
        {
            Type = ItemType.Material;
        }
    }
}
