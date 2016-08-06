using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vRPGContent.Data.Attributes;
using vRPGData.Databases;

namespace vRPGEngine.Databases
{
    public sealed class SpecializationDatabase : Database<SpecializationData, SpecializationDatabase>
    {
        private SpecializationDatabase()
            : base()
        {
            Readonly = true;
        }

        protected override List<SpecializationData> LoadData()
        {
            try
            {
                Logger.Instance.LogFunctionMessage("loading specializations database...");

                var data = vRPGEngine.Instance.Content.Load<SpecializationData[]>("Databases\\specializations");

                Logger.Instance.LogFunctionMessage("load ok!");

                return data.ToList();
            }
            catch (Exception)
            {
                Logger.Instance.LogFunctionWarning("could not load specializations database!");

                return null;
            }
        }
    }
}
