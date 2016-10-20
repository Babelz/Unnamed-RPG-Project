using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vRPGEngine;
using vRPGEngine.Core;

namespace vRPGEngine.Databases
{
    public abstract class Database<TElements, TDatabase> : Singleton<TDatabase> where TDatabase : Database<TElements, TDatabase> where TElements : class
    {
        #region Fields
        private readonly List<TElements> elements;
        #endregion

        #region Properties
        public bool Readonly
        {
            get;
            protected set;
        }
        #endregion

        protected Database()
            : base()
        {
            elements = LoadData();
        }

        protected abstract List<TElements> LoadData();

        protected virtual void SaveData()
        {
        }

        public TElements First(Func<TElements, bool> predicate)
        {
            var result = elements.FirstOrDefault(predicate);

            if (result == null) Logger.Instance.LogWarning("query failed, no results found!");

            return result;
        }
        public IEnumerable<TElements> Select(Func<TElements, bool> predicate)
        {
            var results = elements.Where(predicate);

            if (results.Count() == 0) Logger.Instance.LogWarning("query returned 0 results!");

            return results;
        }

        public void Sync()
        {
            if (Readonly) return;

            SaveData();
        }
    }
}
