using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vRPGEngine;

namespace vRPGData.Databases
{
    public abstract class Database<T> : Singleton<Database<T>> where T : class
    {
        #region Fields
        private readonly List<T> elements;
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

        protected abstract List<T> LoadData();

        protected virtual void SaveData()
        {
        }

        public T Query(Predicate<T> pred)
        {
            foreach (var element in elements) if (pred(element)) return element;

            return null;
        }

        public void Sync()
        {
            if (Readonly) return;

            SaveData();
        }
    }
}
