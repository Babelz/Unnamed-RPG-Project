using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vRPGEngine.Attributes;
using vRPGEngine.Databases;

namespace vRPGEngine.Handlers.Spells
{
    public abstract class MeleeSpellHandler : SpellHandler
    {
        #region Properties
        #endregion

        public MeleeSpellHandler(string name, PowerSource source, float percentageOfSource, int baseDamage)
            : base(SpellDatabase.Instance.First(p => p.Name.ToLower() == name))
        {
        }
    }
}
