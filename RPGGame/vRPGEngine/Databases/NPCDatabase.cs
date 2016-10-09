using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vRPGContent.Data.Characters;
using vRPGEngine.Core;

namespace vRPGEngine.Databases
{
    public sealed class NPCDatabase : Database<NPCData, NPCDatabase>
    {
        private NPCDatabase()
            : base()
        {
            Readonly = true;
        }

        protected override List<NPCData> LoadData()
        {
            try
            {
                Logger.Instance.LogFunctionMessage("loading npc database...");

                var data = Engine.Instance.Content.Load<NPCData[]>("Databases\\npcs");

                Logger.Instance.LogFunctionMessage("load ok!");

                return data.ToList();
            }
            catch (Exception e)
            {
                Logger.Instance.LogFunctionWarning("could not load npc database!");

                Logger.Instance.LogError("exception message: " + e.Message);

                return null;
            }
        }
    }
}
