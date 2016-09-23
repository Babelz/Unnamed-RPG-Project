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

                if (oldValue == health) return; 

                HealthChanged?.Invoke(this, health, oldValue);

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

                if (oldValue == mana) return;

                ManaChanged?.Invoke(this, mana, oldValue);

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

                if (oldValue == focus) return;

                FocusChanged?.Invoke(this, focus, oldValue);

                StatusChanged?.Invoke(this, "Focus");
            }
        }

        public bool Alive
        {
            get
            {
                return health != 0;
            }
        }
        public bool HasFocus
        {
            get
            {
                return focus != 0;
            }
        }
        public bool HasMana
        {
            get
            {
                return mana != 0;
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
