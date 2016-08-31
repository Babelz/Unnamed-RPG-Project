using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using vRPGEngine.ECS.Components;
using vRPGEngine.Core;

namespace vRPGEngine.Combat
{
    public sealed class GlobalCooldownEntry
    {
        public ICharacterController Sender;

        public int Time;
        public int Elapsed;
    }

    public sealed class GlobalCooldownManager : SystemManager<GlobalCooldownManager>
    {
        #region Constants
        private const int BaseGlobalCooldownTime = 1000;
        #endregion

        #region Fields
        private readonly Dictionary<ICharacterController, GlobalCooldownEntry> entries;
        #endregion

        private GlobalCooldownManager()
            : base()
        {
            entries = new Dictionary<ICharacterController, GlobalCooldownEntry>();
        }

        protected override void OnUpdate(GameTime gameTime)
        {
            base.OnUpdate(gameTime);

            foreach (var kvp in entries)
            {
                var entry = kvp.Value;

                entry.Elapsed += gameTime.ElapsedGameTime.Milliseconds;
            }
        }

        public void Trigger(ICharacterController character)
        {
            var time = BaseGlobalCooldownTime;

            entries.Add(character, new GlobalCooldownEntry() { Time = time, Sender = character });
        }

        public int CooldownLeft(ICharacterController character)
        {
            if (!entries.ContainsKey(character)) return 0;
            
            var entry = entries[character];

            return MathHelper.Clamp(entry.Time - entry.Elapsed, 0, entry.Time);
        }

        public bool IsInCooldown(ICharacterController character)
        {
            if (!entries.ContainsKey(character)) return false;
            
            if (CooldownLeft(character) == 0)
            {
                entries.Remove(character);

                return false;
            }

            return true;
        }
    }
}
