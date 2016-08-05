﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vRPGEngine.Data.Spells
{
    [Serializable()]
    public sealed class Spell
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
        public string IconName
        {
            get;
            set;
        }
        public string Description
        {
            get;
            set;
        }
        #endregion
    }
}
