using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vRPGEngine.Attributes
{
    public sealed class Statuses
    {
        #region Properties
        public int Health
        {
            get;
            set;
        }
        public int Mana
        {
            get;
            set;
        }
        public int Focus
        {
            get;
            set;
        }
        #endregion

        public Statuses()
        {
        }
    }
}
