using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vRPGContent.Data.Characters.Enums;

namespace vRPGContent.Data.Characters
{
    [Serializable()]
    public class NPC
    {
        #region Properties
        public string Name
        {
            get;
            set;
        }
        public int ID
        {
            get;
            set;
        }
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
        public int Level
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
        public float CritChance
        {
            get;
            set;
        }
        public Factions Faction
        {
            get;
            set;
        }
        public int[] LootList
        {
            get;
            set;
        }
        public int[] SpellList
        {
            get;
            set;
        }
        public int[] SpellPriority
        {
            get;
            set;
        }
        public string HandlerName
        {
            get;
            set;
        }
        public string TextureSetName
        {
            get;
            set;
        }
        #endregion

        public NPC()
        {
        }
    }
}
