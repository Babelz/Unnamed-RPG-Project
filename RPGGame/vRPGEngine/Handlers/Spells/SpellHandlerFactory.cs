using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vRPGContent.Data.Spells;

namespace vRPGEngine.Handlers.Spells
{
    public static class SpellHandlerFactory
    {
        #region Fields
        private static readonly Dictionary<Spell, ICloneable> activators;

        private static readonly string handlersNamespace;
        #endregion

        static SpellHandlerFactory()
        {
            activators          = new Dictionary<Spell, ICloneable>();
            handlersNamespace   = "vRPGEngine.Handlers.Spells.";
        }

        public static SpellHandler Create(Spell spell)
        {
            if (string.IsNullOrEmpty(spell.HandlerName)) return null;

            if (activators.ContainsKey(spell)) return activators[spell].Clone() as SpellHandler;

            // Not created yet, use reflection.
            var type = Type.GetType(handlersNamespace + spell.HandlerName);
            
            if (type == null)
            {
                Logger.Instance.LogFunctionWarning(string.Format("could not create handler for spell \"{0}\"", spell.ToString()));

                return null;
            }

            var handler = Activator.CreateInstance(type) as SpellHandler;

            activators.Add(spell, handler);

            return handler;
        }
    }
}
