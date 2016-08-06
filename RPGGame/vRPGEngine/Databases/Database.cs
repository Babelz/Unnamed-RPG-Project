using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vRPGEngine;

namespace vRPGData.Databases
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

        public TElements Query(Predicate<TElements> pred)
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
