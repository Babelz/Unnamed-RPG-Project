﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vRPGContent.Data.Items.Enums;

namespace vRPGContent.Data.Items
{
    /// <summary>
    /// Items that are associated with some quest.
    /// </summary>
    [Serializable()]
    public sealed class QuestItem : Item
    {
        #region Properties
        public int QuestID
        {
            get;
            set;
        }
        #endregion

        public QuestItem()
            : base()
        {
            Type            = ItemType.QuestItem;
            VendorValue     = 0;                // Can't sell these to vendors.
        }
    }
}
