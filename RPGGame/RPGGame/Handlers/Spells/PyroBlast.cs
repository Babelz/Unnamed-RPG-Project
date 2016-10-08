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
    public sealed class PyroBlast : MissileSpellHandler
    {
        #region Fields
        private SpriteRenderer renderer;
        private RangedDamageResults results;
        #endregion

        public PyroBlast() 
            : base("pyro blast")
        {
        }

        #region Event handlers
        private void Controller_CastSuccessful(ref RangedDamageResults results)
        {
            this.results = results;

            renderer                = Owner.AddComponent<SpriteRenderer>();
            renderer.Flags          = RenderFlags.AutomaticDepth;
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
        }
        
        protected override void OnHit()
        {
            // Deal damage.

            // Add debuff.
        }

        public override void Update(GameTime gameTime)
        {
            if (Sensor != null)
            {
                renderer.Sprite.Position = ConvertUnits.ToDisplayUnits(Sensor.Position);

                var dir = (Target.FirstComponentOfType<Transform>().Position - ConvertUnits.ToDisplayUnits(Sensor.Position));
                dir.Normalize();

                Sensor.LinearVelocity = dir * MissileVelocity;
            }
        }

        public override SpellHandler Clone()
        {
            return new PyroBlast();
        }
    }
}
