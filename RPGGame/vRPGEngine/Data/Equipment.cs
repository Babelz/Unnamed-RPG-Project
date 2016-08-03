using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vRPGEngine.Data
{
    [Serializable()]
    public abstract class Equipment : Item
    {
        #region Properties
        public int Stamina
        {
            get;
            set;
        }
        public int Intellect
        {
            get;
            set;
        }
        public int Endurance
        {
            get;
            set;
        }
        public int Strength
        {
            get;
            set;
        }
        public int Agility
        {
            get;
            set;
        }
        public int MeleePower
        {
            get;
            set;
        }
        public int SpellPower
        {
            get;
            set;
        }
        /// <summary>
        /// Mana per 5s
        /// </summary>
        public int MP5
        {
            get;
            set;
        }
        /// <summary>
        /// Endurance per 5s
        /// </summary>
        public int EP5
        {
            get;
            set;
        }
        /// <summary>
        /// Health per 5s
        /// </summary>
        public int HP5
        {
            get;
            set;
        }
        public float CritPercent
        {
            get;
            set;
        }
        public float DefencePercent
        {
            get;
            set;
        }
        public float BlockPercent
        {
            get;
            set;
        }
        public float DodgePercent
        {
            get;
            set;
        }
        public float ParryPercent
        {
            get;
            set;
        }
        public int Haste
        {
            get;
            set;
        }
        public string BehaviourName
        {
            get;
            set;
        }
        #endregion

        public Equipment()
            : base()
        {
            BehaviourName = string.Empty;
        }
    }
}
