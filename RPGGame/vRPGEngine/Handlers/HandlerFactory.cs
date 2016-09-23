using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using vRPGEngine.Core;
using vRPGEngine.Interfaces;

namespace vRPGEngine.Handlers
{
    public abstract class HandlerFactory<TFactory, TProduct> : Singleton<TFactory> where TFactory : HandlerFactory<TFactory, TProduct> where TProduct : class, IGenericCloneable<TProduct>
    {
        #region Static fields
        private static readonly Type[] Types;
        #endregion

        #region Fields
        private readonly Dictionary<string, IGenericCloneable<TProduct>> activators;
        #endregion

        static HandlerFactory()
        {
            var assemblies          = AppDomain.CurrentDomain.GetAssemblies();
            var types               = new List<Type>();
            var definedAssemblies   = GameSetting.Engine.HandlerAssemblies;
            
            foreach (var assembly in assemblies.Where(a => definedAssemblies.Contains(a.FullName.Split(new char[] { ',' }).First().Trim())))
                types.AddRange(assembly.GetTypes().Where(a => a.IsSubclassOf(typeof(TProduct))));

            Types = types.ToArray();
        }

        protected HandlerFactory()
        {
            activators = new Dictionary<string, IGenericCloneable<TProduct>>();
        }

        public TProduct Create(string handlerName)
        {
            if (string.IsNullOrEmpty(handlerName)) return null;

            if (activators.ContainsKey(handlerName)) return activators[handlerName].Clone();

            // Not created yet, use reflection.
            var type        = Types.FirstOrDefault(t => t.Name == handlerName);

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
