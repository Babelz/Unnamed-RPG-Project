using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vRPGContent.Data.Items;
using vRPGEngine.Core;

namespace vRPGEngine.Databases
{
    public sealed class WeaponDatabase : Database<Weapon, WeaponDatabase>
    {
        protected override List<Weapon> LoadData()
        {
            try
            {
                Logger.Instance.LogFunctionMessage("loading weapon database...");

                var data = Engine.Instance.Content.Load<Weapon[]>("Databases\\weapons");

                Logger.Instance.LogFunctionMessage("load ok!");

                return data.ToList();
            }
            catch (Exception e)
            {
                Logger.Instance.LogFunctionWarning("could not load weapon database!");

                Logger.Instance.LogError("exception message: " + e.Message);

                return null;
            }
        }
    }
}
