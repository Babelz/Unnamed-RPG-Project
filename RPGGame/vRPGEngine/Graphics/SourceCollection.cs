using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vRPGEngine.Graphics
{
    public class SourceCollection 
    {
        #region Fields
        private readonly Dictionary<string, Rectangle> sources;
        #endregion

        #region Indexer
        public Rectangle this[string name]
        {
            get
            {
                Rectangle value;

                sources.TryGetValue(name, out value);

                return value;
            }
        }
        #endregion

        public SourceCollection(Dictionary<string, Rectangle> sources)
        {
            Debug.Assert(sources != null);

            this.sources = sources;
        }
        
        public bool Contains(string name)
        {
            return sources.ContainsKey(name);
        }
    }
}
