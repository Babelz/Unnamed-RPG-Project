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

        #region Events
        public event StausesChangedEventHandler HealthChanged;
        public event StausesChangedEventHandler ManaChanged;
        public event StausesChangedEventHandler FocusChanged;

        public event StatusesEventHandler StatusChanged;
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
                var oldValue = health;

                health = value < 0 ? 0 : value;

                if (oldValue != health) HealthChanged?.Invoke(this, health, oldValue);

                StatusChanged?.Invoke(this, "Health");
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
                var oldValue = mana;

                mana = value < 0 ? 0 : value;

                if (oldValue != mana) HealthChanged?.Invoke(this, mana, oldValue);

                StatusChanged?.Invoke(this, "Mana");
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
                var oldValue = focus;

                focus = value < 0 ? 0 : value;

                if (oldValue != focus) HealthChanged?.Invoke(this, focus, oldValue);

                StatusChanged?.Invoke(this, "Focus");
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

        public delegate void StausesChangedEventHandler(Statuses sender, int newValue, int oldValue);
        public delegate void StatusesEventHandler(Statuses sender, string name);
    }
}
