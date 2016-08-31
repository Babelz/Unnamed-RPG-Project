using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vRPGEngine.Core;

namespace vRPGEngine.Handlers
{
    public abstract class HandlerFactory<TFactory, TProduct> : Singleton<TFactory> where TFactory : HandlerFactory<TFactory, TProduct> where TProduct : class, ICloneable
    {
        #region Fields
        private readonly Dictionary<string, ICloneable> activators;

        private readonly string handlersNamespace;
        #endregion

        protected HandlerFactory(string handlersNamespace)
        {
            this.handlersNamespace = handlersNamespace;

            activators = new Dictionary<string, ICloneable>();
        }

        public TProduct Create(string handlerName)
        {
            if (string.IsNullOrEmpty(handlerName)) return null;

            if (activators.ContainsKey(handlerName)) return activators[handlerName].Clone() as TProduct;

            // Not created yet, use reflection.
            var type = Type.GetType(handlersNamespace + handlerName);

            if (type == null)
            {
                Logger.Instance.LogFunctionWarning(string.Format("could not create handler for \"{0}\"", handlerName));

                return null;
            }

            var handler = Activator.CreateInstance(type) as TProduct;

            activators.Add(handlerName, handler);

            return handler;
        }
    }
}
