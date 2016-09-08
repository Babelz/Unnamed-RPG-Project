using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using vRPGEngine.Core;

namespace vRPGEngine.Handlers
{
    public abstract class HandlerFactory<TFactory, TProduct> : Singleton<TFactory> where TFactory : HandlerFactory<TFactory, TProduct> where TProduct : class, ICloneable
    {
        #region Static fields
        private static readonly Type[] Types;
        #endregion

        #region Fields
        private readonly Dictionary<string, ICloneable> activators;
        #endregion

        #region Properties
        public string HandlersNamespace
        {
            get;
            set;
        }
        #endregion

        static HandlerFactory()
        {
            var assemblies  = AppDomain.CurrentDomain.GetAssemblies();
            var types       = new List<Type>();

            foreach (var assembly in assemblies) types.AddRange(assembly.GetTypes().Where(a => a.IsSubclassOf(typeof(TProduct))));

            Types = types.ToArray();
        }

        protected HandlerFactory()
        {
            activators = new Dictionary<string, ICloneable>();
        }

        public TProduct Create(string handlerName)
        {
            if (string.IsNullOrEmpty(handlerName)) return null;

            if (activators.ContainsKey(handlerName)) return activators[handlerName].Clone() as TProduct;

            // Not created yet, use reflection.
            var fullName    = HandlersNamespace + handlerName;
            var type        = Types.FirstOrDefault(t => t.FullName == fullName);

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
