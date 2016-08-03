using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace vRPGEngine.Data
{
    /// <summary>
    /// Base class for all the items. Can be 
    /// used as a instance for trash items.
    /// </summary>
    [Serializable()]
    public class Item
    {
        #region Properties
        public int ID
        {
            get;
            set;
        }
        public string Name
        {
            get;
            set;
        }
        public string Description
        {
            get;
            set;
        }
        public string TextureSetName
        {
            get;
            set;
        }
        public int VendorValue
        {
            get;
            set;
        }
        public Quality Quality
        {
            get;
            set;
        }

        [XmlIgnore()]
        public ItemType Type
        {
            get;
            protected set;
        }
        #endregion

        public Item()
        {
            VendorValue     = 1;
            Name            = string.Empty;
            Description     = string.Empty;
            TextureSetName  = string.Empty;
        }
    }
}
