using FarseerPhysics;
using Microsoft.Xna.Framework;
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
using vRPGEngine.Handlers.Spells;

namespace RPGGame.Handlers.Spells
{
    public sealed class Fireball : MissileSpellHandler
    {
        #region Fields
        private Transform target;
        private SpriteRenderer renderer;
        private RangedDamageResults results;
        #endregion

        public Fireball() 
            : base("fireball")
        {
        }

        #region Event handlers
        private void Controller_CastSuccessful(ref RangedDamageResults results)
        {
            Send();

            this.results = results;

            target                  = Target.FirstComponentOfType<Transform>();

            renderer                = Owner.AddComponent<SpriteRenderer>();
            renderer.Flags          = RenderFlags.AutomaticDepth;
            renderer.Sprite.Layer   = Layers.Middle;
            renderer.Sprite.Texture = DefaultValues.MissingTexture;
            renderer.Sprite.ScaleTo(new Vector2(32.0f));

        }
        private void Controller_OnEndCast(bool interrupted)
        {
        }
        #endregion

        protected override void OnUse()
        {
            var controller      = Owner.FirstComponentOfType<ICharacterController>().RangedDamageController;

            controller.CastSuccessful   += Controller_CastSuccessful;
            controller.OnEndCast        += Controller_OnEndCast;

            controller.BeginCast(Spell, PowerSource.SpellPower, 1.25f, 10);
        }
        
        protected override void OnHit()
        {
            // Deal damage.

            // Add debuff.
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (Sensor != null)
            {
                renderer.Sprite.Position = ConvertUnits.ToDisplayUnits(Sensor.Position);

                var dir = (target.Position - ConvertUnits.ToDisplayUnits(Sensor.Position));
                dir.Normalize();

                Sensor.LinearVelocity = dir * MissileVelocity;
            }
        }

        public override SpellHandler Clone()
        {
            return new Fireball();
        }
    }
}
