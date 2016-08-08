using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vRPGContent.Data.Characters
{
    [Serializable()]
    public sealed class NPCQuestGiver : NPC
    {
        #region Properties
        public int[] Quests
        {
            get;
            set;
        }
        #endregion

        NPCQuestGiver()
            : base()
        {
        }
    }
}
