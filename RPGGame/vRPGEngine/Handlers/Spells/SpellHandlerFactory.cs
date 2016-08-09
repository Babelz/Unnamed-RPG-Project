using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vRPGContent.Data.Spells;

namespace vRPGEngine.Handlers.Spells
{
    public sealed class SpellHandlerFactory : HandlerFactory<SpellHandlerFactory, Spell, SpellHandler>
    {
        private SpellHandlerFactory()
            : base("vRPGEngine.Handlers.Spells.")
        {
        }
    }
}
