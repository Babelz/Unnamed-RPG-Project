using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vRPGContent.Data.Attributes;

namespace vRPGEngine.Databases
{
    public sealed class PassiveSpecializationBuffDatabase : Database<PassiveSpecializationBuff, PassiveSpecializationBuffDatabase>
    {
        private PassiveSpecializationBuffDatabase()
            : base()
        {
            Readonly = true;
        }

        protected override List<PassiveSpecializationBuff> LoadData()
        {
            try
            {
                Logger.Instance.LogFunctionMessage("loading passive spec buffs database...");

                var data = Engine.Instance.Content.Load<PassiveSpecializationBuff[]>("Databases\\spec passives");

                Logger.Instance.LogFunctionMessage("load ok!");

                return data.ToList();
            }
            catch (Exception)
            {
                Logger.Instance.LogFunctionWarning("could not load passive spec buffs database!");

                return null;
            }
        }
    }
}
