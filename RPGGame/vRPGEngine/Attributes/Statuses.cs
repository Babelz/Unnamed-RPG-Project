using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vRPGEngine.Attributes.Specializations;
using vRPGEngine.Specializations;

namespace vRPGEngine.Attributes
{
    public sealed class Statuses
    {
        #region Fields
        private int health;
        private int mana;
        private int focus;
        #endregion

        #region Properties
        public int Health
        {
            get
            {
                return health;
            }
            set
            {
                health = value < 0 ? 0 : value;
            }
        }
        public int Mana
        {
            get
            {
                return mana;
            }
            set
            {
                mana = value < 0 ? 0 : value;
            }
        }
        public int Focus
        {
            get
            {
                return focus;
            }
            set
            {
                focus = value < 0 ? 0 : value;
            }
        }
        #endregion

        public Statuses()
        {
        }

        public void Initialize(Specialization specialization)
        {
            Health  = specialization.TotalHealth();
            Mana    = specialization.TotalMana();
            Focus   = specialization.TotalFocus();
        }
    }
}
