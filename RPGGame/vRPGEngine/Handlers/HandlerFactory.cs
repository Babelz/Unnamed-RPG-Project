using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vRPGEngine.Handlers
{
    public abstract class HandlerFactory<TFactory, TElement, TProduct> : Singleton<TFactory> where TFactory : HandlerFactory<TFactory, TElement, TProduct> where TProduct : class, ICloneable
    {
        #region Fields
        private readonly Dictionary<TElement, ICloneable> activators;

        private readonly string handlersNamespace;
        #endregion

        protected HandlerFactory(string handlersNamespace)
        {
            this.handlersNamespace = handlersNamespace;

            activators = new Dictionary<TElement, ICloneable>();
        }

        public TProduct Create(TElement element, string handlerName)
        {
            if (string.IsNullOrEmpty(handlerName)) return null;

            if (activators.ContainsKey(element)) return activators[element].Clone() as TProduct;

            // Not created yet, use reflection.
            var type = Type.GetType(handlersNamespace + handlerName);

            if (type == null)
            {
                Logger.Instance.LogFunctionWarning(string.Format("could not create handler for \"{0}\"", element.ToString()));

                return null;
            }

            var handler = Activator.CreateInstance(type) as TProduct;

            activators.Add(element, handler);

            return handler;
        }
    }
}
