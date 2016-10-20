using FarseerPhysics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vRPGEngine;
using vRPGEngine.Attributes;
using vRPGEngine.Core;
using vRPGEngine.ECS;
using vRPGEngine.ECS.Components;
using vRPGEngine.Graphics;
using vRPGEngine.Handlers.Spells;

namespace RPGGame.Handlers.Spells
{
    public sealed class Fireball : RangedSpellHandler
    {
        #region Fields
        private SpriteRenderer renderer;
        
        private Transform target;
        #endregion

        public Fireball() 
            : base("fireball", PowerSource.SpellPower, 0.75f, 12)
        {
        }

        protected override void Send()
        {
            base.Send();

            renderer.Destroy();
        }

        protected override void Cast()
        {
            renderer = new SpriteRenderer();
            renderer.Sprite.
        }
        protected override void Tick(GameTime gameTime)
        {
        }

        public override SpellHandler Clone()
        {
            return new Fireball();
        }
    }
}
