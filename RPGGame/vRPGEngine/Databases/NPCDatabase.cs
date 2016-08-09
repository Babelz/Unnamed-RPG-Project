using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vRPGContent.Data.Characters;

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

                var data = vRPGEngine.Instance.Content.Load<NPCData[]>("Databases\\npcs");

                Logger.Instance.LogFunctionMessage("load ok!");

                return data.ToList();
            }
            catch (Exception)
            {
                Logger.Instance.LogFunctionWarning("could not load npc database!");

                return null;
            }
        }
    }
}
