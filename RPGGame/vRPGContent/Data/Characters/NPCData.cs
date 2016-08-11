using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vRPGContent.Data.Attributes;
using vRPGContent.Data.Characters.Enums;

namespace vRPGContent.Data.Characters
{
    [Serializable()]
    public class NPCData : AttributesData
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
        public string SpecializationName
        {
            get;
            set;
        }
        #endregion

        public NPCData()
            : base()
        {
        }
        public NPCData(NPCData other)
            : base()
        {
            Name                    = other.Name;
            ID                      = other.ID;
            Faction                 = other.Faction;
            LootList                = other.LootList?.ToArray();
            SpellList               = other.SpellList?.ToArray();
            SpellPriority           = other.SpellPriority?.ToArray();
            HandlerName             = other.HandlerName;

            CopyAttributes(other);
        }

        public void CopyAttributes(AttributesData other)
        {
            Armor                       = other.Armor;
            Stamina                     = other.Stamina;
            Intellect                   = other.Intellect;
            Endurance                   = other.Endurance;
            Strength                    = other.Strength;
            Agility                     = other.Agility;
            Mp5                         = other.Mp5;
            Hp5                         = other.Hp5;
            Fp5                         = other.Fp5;
            Haste                       = other.Haste;
            CriticalHitPercent          = other.CriticalHitPercent;
            DefenceRatingPercent        = other.DefenceRatingPercent;
            BlockRatingPercent          = other.BlockRatingPercent;
            DodgeRatingPercent          = other.DodgeRatingPercent;
            ParryRatingPercent          = other.ParryRatingPercent;
            MovementSpeedPercent        = other.MovementSpeedPercent;
            PureMeleePower              = other.PureMeleePower;
            PureSpellPower              = other.PureSpellPower;
            HealthPercentModifier       = other.HealthPercentModifier;
            MeeleePowerPercentModifier  = other.MeeleePowerPercentModifier;
        }
    }
}
