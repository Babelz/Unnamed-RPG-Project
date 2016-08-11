using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vRPGContent.Data.Items.Enums;

namespace vRPGContent.Data.Spells
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
        public Elements Element
        {
            get;
            set;
        }
        public string HandlerName
        {
            get;
            set;
        }
        public int Cost
        {
            get;
            set;
        }
        public int Cooldown
        {
            get;
            set;
        }
        public int CastTime
        {
            get;
            set;
        }
        public float Range
        {
            get;
            set;
        }
        public SpellCostType CostType
        {
            get;
            set;
        }
        public Currencies Consumes
        {
            get;
            set;
        }
        #endregion

        public Spell()
        {
        }

        public override string ToString()
        {
            return string.Format("{0} - {1}", Name, HandlerName);
        }
        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }
    }
}
