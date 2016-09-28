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
        private readonly string name;
        
        private readonly WeakGetterDelegate get;
        private readonly WeakSetterDelegate set;
        #endregion

        #region Properties
        public string Name
        {
            get
            {
                return name;
            }
        }
        public bool HasGetter
        {
            get
            {
                return get != null;
            }
        }
        public bool HasSetter
        {
            get
            {
                return set != null;
            }
        }
        #endregion

        #region Events
        public event ValueChangedEventHandler ValueChanged;
        #endregion

        public DependencyProperty(string name, WeakGetterDelegate get = null, WeakSetterDelegate set = null)
        {
            this.name   = name;
            this.get    = get;
            this.set    = set;
        }

        public void Set(object value)
        {
            set(value);

            ValueChanged?.Invoke(this);
        }
        public object Get()
        {
            return get();
        }
    }
    
    public abstract class DependencyPropertyContainer
    {
        #region Fields
        private readonly List<DependencyProperty> properties;
        #endregion

        public DependencyPropertyContainer()
        {
            properties = new List<DependencyProperty>();
        }

        protected void RegisterProperty(string name, WeakGetterDelegate get = null, WeakSetterDelegate set = null)
        {
            if (properties.FirstOrDefault(p => p.Name == name) != null)
            {
                Logger.Instance.LogFunctionWarning("duplicated property " + name);

                return;
            }

#if DEBUG
            if (GetType().GetProperties(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance).FirstOrDefault(p => p.Name == name) == null)
                Logger.Instance.LogError("property not found, pname: " + name);
#endif

            properties.Add(new DependencyProperty(name, get, set));
        }
        protected void UnregisterProperty(string name)
        {
            var property = properties.FirstOrDefault(p.Name == name);

            properties.Remove(property);
        }

        public bool ReadProperty(string name, ref object results)
        {
            DependencyProperty property = properties.FirstOrDefault(p => p.Name == name);

            if (property != null)
            {
                if (property.HasGetter) results = property.Get();

                return true;
            }

            return false;
        }
        public bool WriteProperty(string name, object value)
        {
            DependencyProperty property = properties.FirstOrDefault(p => p.Name == name);

            try
            {
                if (property != null)
                {
                    if (property.HasSetter) property.Set(value);

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
            DependencyProperty property = properties.FirstOrDefault(p => p.Name == name);
            
            return property;
        }

        public bool HasProperty(string name)
        {
            return properties.FirstOrDefault(p => p.Name == name) != null;
        }
    }
}
