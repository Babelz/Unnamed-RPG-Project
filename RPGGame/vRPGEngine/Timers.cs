using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vRPGEngine
{
    public sealed class Timer
    {
        public int Elapsed;
    }

    public sealed class Timers
    {
        #region Fields
        private readonly Dictionary<object, Timer> timers;
        #endregion

        public Timers()
        {
            timers = new Dictionary<object, Timer>();
        }

        public void Add(object key, int initialValue = 0)
        {
            timers.Add(key, new Timer() { Elapsed = initialValue });
        }
        public void Remove(object key)
        {
            timers.Remove(key);
        }

        public int Read(object key)
        {
            return timers[key].Elapsed;
        }
        public int Reset(object key)
        {
            var value = timers[key].Elapsed;

            timers[key].Elapsed = 0;

            return value;
        }

        public void Update(GameTime gameTime)
        {
            foreach (var kvp in timers) kvp.Value.Elapsed += gameTime.ElapsedGameTime.Milliseconds;
        }
    }
}