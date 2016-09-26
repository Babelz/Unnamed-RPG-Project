using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vRPGEngine.Core;

namespace vRPGEngine.HUD
{
    public delegate object WeakGetterDelegate();
    public delegate void WeakSetterDelegate(object value);

    public delegate T StrongGetterDelegate<T>();
    public delegate void StrongSetterDelegate<T>(T value);

    public sealed class DependencyProperty 
    {
        #region Fields
        public readonly bool CanRead;
        public readonly bool CanWrite;
        
        public readonly WeakGetterDelegate Read;
        public readonly WeakSetterDelegate Write;
        #endregion

        public DependencyProperty(WeakGetterDelegate get = null, WeakSetterDelegate set = null)
        {
            Read  = get;
            Write = set;

            CanRead  = get != null;
            CanWrite = set != null;
        }
    }
    
    public abstract class DependencyPropertyContainer
    {
        #region Fields
        private readonly Dictionary<string, DependencyProperty> properties;
        #endregion

        public DependencyPropertyContainer()
        {
            properties = new Dictionary<string, DependencyProperty>();
        }

        protected void ValidateProperties(Type type)
        {
#if DEBUG
            var typeProperties = type.GetProperties();

            foreach (var property in properties)
            {
                if (typeProperties.FirstOrDefault(p => p.Name == property.Key) == null)
                    Logger.Instance.LogError("property not found, typo or forgot to register? pname: " + property.Key);
            }
#endif
        }

        protected void RegisterProperty(string name, WeakGetterDelegate get = null, WeakSetterDelegate set = null)
        {
            if (properties.ContainsKey(name))
            {
                Logger.Instance.LogFunctionWarning("duplicated property " + name);

                return;
            }

            properties.Add(name, new DependencyProperty(get, set));
        }
        protected void UnregisterProperty(string name)
        {
            if (properties.ContainsKey(name)) properties.Remove(name);
        }

        public bool ReadProperty(string name, ref object results)
        {
            DependencyProperty property = null;

            if (properties.TryGetValue(name, out property))
            {
                if (property.CanRead) results = property.Read();

                return true;
            }

            return false;
        }
        public bool WriteProperty(string name, object value)
        {
            DependencyProperty property = null;

            try
            {
                if (properties.TryGetValue(name, out property))
                {
                    if (property.CanWrite) property.Write(value);

                    return true;
                }

                return false;
            }
            catch (Exception e)
            {
                Logger.Instance.LogWarning("could not set value, " + e.Message);

                return false;
            }
        }

        public bool ReadProperty<T>(string name, ref T results)
        {
            object nonGenericResults = null;

            if (ReadProperty(name, ref nonGenericResults))
            {
                if (nonGenericResults.GetType() == typeof(T))
                {
                    results = (T)nonGenericResults;

                    return true;
                }
            }

            results = default(T);

            return false;
        }
        public bool WriteProperty<T>(string name, T value)
        {
            object nonGenericValue = value;

            return WriteProperty(name, nonGenericValue);
        }

        public DependencyProperty GetProperty(string name)
        {
            DependencyProperty property = null;

            properties.TryGetValue(name, out property);

            return property;
        }

        public bool HasProperty(string name)
        {
            return properties.ContainsKey(name);
        }
    }
}
