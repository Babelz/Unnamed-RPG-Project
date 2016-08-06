using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vRPGContent.Data.Spells;

namespace vRPGEngine.Databases
{
    public sealed class SpellDatabase : Database<Spell, SpellDatabase>
    {
        private SpellDatabase()
            : base()
        {
            Readonly = true;
        }

        protected override List<Spell> LoadData()
        {
            try
            {
                Logger.Instance.LogFunctionMessage("loading spells database...");

                var data = vRPGEngine.Instance.Content.Load<Spell[]>("Databases\\spells");

                Logger.Instance.LogFunctionMessage("load ok!");

                return data.ToList();
            }
            catch (Exception)
            {
                Logger.Instance.LogFunctionWarning("could not load spells database!");

                return null;
            }
        }
    }
}
