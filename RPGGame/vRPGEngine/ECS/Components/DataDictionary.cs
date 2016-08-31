using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vRPGEngine.Core;

namespace vRPGEngine.ECS.Components
{
    public sealed class DataDictionary : Component<DataDictionary>
    {
        #region Fields
        private readonly Dictionary<string, object> dict;
        #endregion

        public DataDictionary()
            : base()
        {
            dict = new Dictionary<string, object>();
        }

        public void Add(string key, object value)
        {
            Debug.Assert(!string.IsNullOrEmpty(key));

            if (dict.ContainsKey(key)) throw new vRPGEngineException("key already exists");
        }
        public object Remove(string key)
        {
            Debug.Assert(!string.IsNullOrEmpty(key));

            if (dict.ContainsKey(key))
            {
                var value = dict[key];

                dict.Remove(key);

                return value;
            }

            return null;
        }
        public bool Contains(string key)
        {
            return dict.ContainsKey(key);
        }

        public object Read(string key)
        {
            Debug.Assert(!string.IsNullOrEmpty(key));

            if (!dict.ContainsKey(key)) return null;

            return dict[key];
        }
    }
}
