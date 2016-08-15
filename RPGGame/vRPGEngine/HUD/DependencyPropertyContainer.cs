using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vRPGEngine.HUD
{
    public delegate object GetterDelegate();

    public delegate void SetterDelegate(object value);
    
    public sealed class DependencyProperty 
    {
        public readonly bool CanRead;
        public readonly bool CanWrite;
        
        public readonly GetterDelegate Read;
        public readonly SetterDelegate Write;

        public DependencyProperty(GetterDelegate get = null, SetterDelegate set = null)
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

        protected void RegisterProperty(string name, GetterDelegate get = null, SetterDelegate set = null)
        {
            if (properties.ContainsKey(name))
            {
                Logger.Instance.LogFunctionWarning("duplicated property " + name);

                return;
            }

            properties.Add(name, new DependencyProperty(get, set));
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

        public DependencyProperty GetProperty(string name)
        {
            DependencyProperty property = null;

            properties.TryGetValue(name, out property);

            return property;
        }
    }
}
