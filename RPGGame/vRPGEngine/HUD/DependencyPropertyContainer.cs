using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using vRPGEngine.Core;

namespace vRPGEngine.HUD
{
    public delegate object WeakGetterDelegate();
    public delegate void WeakSetterDelegate(object value);

    public delegate T StrongGetterDelegate<T>();
    public delegate void StrongSetterDelegate<T>(T value);

    public delegate void ValueChangedEventHandler(DependencyProperty property);

    public sealed class DependencyProperty 
    {
        #region Fields
        public readonly WeakGetterDelegate Get;
        public readonly WeakSetterDelegate Set;
        #endregion
        
        public DependencyProperty(WeakGetterDelegate get = null, WeakSetterDelegate set = null)
        {
            Get    = get;
            Set    = set;
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

        protected void RegisterProperty(string name, WeakGetterDelegate get = null, WeakSetterDelegate set = null)
        {
            if (properties.ContainsKey(name))
            {
                Logger.Instance.LogFunctionWarning("duplicated property " + name);

                return;
            }

#if DEBUG
            if (GetType().GetProperties(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance).FirstOrDefault(p => p.Name == name) == null)
                Logger.Instance.LogError("property not found, pname: " + name);
#endif

            properties.Add(name, new DependencyProperty(get, set));
        }
        protected void UnregisterProperty(string name)
        {
            properties.Remove(name);
        }

        public bool ReadProperty(string name, ref object results)
        {
            DependencyProperty property = null;

            properties.TryGetValue(name, out property);
            
            if (property != null)
            {
                if (property.Get != null) results = property.Get();

                return true;
            }

            return false;
        }
        public bool WriteProperty(string name, object value)
        {
            DependencyProperty property = null;

            properties.TryGetValue(name, out property);

            try
            {
                if (property != null)
                {
                    if (property.Set != null) property.Set(value);

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
            return GetProperty(name) != null;
        }
    }
}
