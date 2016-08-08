using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vRPGContent.Data.Quests.Enums;

namespace vRPGContent.Data.Quests
{
    [Serializable()]
    public sealed class Quest
    {
        #region Fields
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
        public QuestType Type
        {
            get;
            set;
        }
        public string[] Description
        {
            get;
            set;
        }
        public int[] ItemRewards
        {
            get;
            set;
        }
        public int MoneyReward
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

        public Quest()
        {
        }
    }
}
