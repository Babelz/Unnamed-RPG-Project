using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vRPGContent.Data.Spells;
using vRPGEngine.Attributes.Enums;
using vRPGEngine.ECS;

namespace vRPGEngine.Attributes.Spells
{
    public abstract class Buff
    {
        #region Properties
        public string Name
        {
            get;
            private set;
        }
        public int Elapsed
        {
            get;
            private set;
        }
        public int Time
        {
            get;
            private set;
        }
        public int TimeLeft
        {
            get
            {
                var value = Time - Elapsed;

                if (value < 0) return 0;

                return value;
            }
        }
        public bool CanStack
        {
            get;
            private set;
        }
        public int Stacks
        {
            get;
            protected set;
        }

        public Spell FromSpell
        {
            get;
            private set;
        }

        public BuffType Type
        {
            get;
            private set;
        }
        #endregion

        protected Buff(string name, int time, bool canStack, Spell fromSpell, BuffType type)
        {
            Name        = name;
            Time        = time;
            FromSpell   = fromSpell;
            CanStack    = canStack;
            Type        = type;
        }

        public abstract void Apply(Entity owner);
        public abstract void Remove(Entity owner);

        public void Update(GameTime gameTime)
        {
            Elapsed += gameTime.ElapsedGameTime.Milliseconds;
        }
    }
}
