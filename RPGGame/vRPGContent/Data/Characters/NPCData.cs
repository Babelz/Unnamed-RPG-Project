using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vRPGContent.Data.Characters.Enums;

namespace vRPGContent.Data.Characters
{
    [Serializable()]
    public class NPCData
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
        #endregion

        public NPCData()
        {
        }
        public NPCData(NPCData other)
        {
            Name            = other.Name;
            ID              = other.ID;
            Health          = other.Health;
            Mana            = other.Mana;
            Focus           = other.Focus;
            Level           = other.Level;
            MeleePower      = other.MeleePower;
            SpellPower      = other.SpellPower;
            CritChance      = other.CritChance;
            Faction         = other.Faction;
            LootList        = other.LootList?.ToArray();
            SpellList       = other.SpellList?.ToArray();
            SpellPriority   = other.SpellPriority?.ToArray();
            HandlerName     = other.HandlerName;
        }
    }
}
