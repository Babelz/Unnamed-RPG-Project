using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vRPGContent.Data.Spells;
using vRPGEngine.ECS;

namespace vRPGEngine.Handlers.Spells
{
    public abstract class SpellHandler
    {
        #region Fields
        public string Name
        {
            get;
            private set;
        }
        public Spell Spell
        {
            get;
            private set;
        }
        public int CooldownElapsed
        {
            get;
            private set;
        }
        public Entity Owner
        {
            get;
            private set;
        }
        public bool Working
        {
            get;
            private set;
        }
        #endregion

        protected SpellHandler(string name, Spell spell)
        {
            Name        = name;
            Spell       = spell;
        }
        
        public abstract void Use(Entity owner);

        public virtual void Update(GameTime gameTime)
        {
        }
        public virtual void Draw(SpriteBatch spriteBatch)
        {
        }
    }

    public abstract class MissileSpellHandler  : SpellHandler
    {
        #region Properties
        #endregion

        protected MissileSpellHandler(string name, Spell spell)
            : base(name, spell)
        {
        }

        public override void Use(Entity owner)
        {
        }
    }
}
