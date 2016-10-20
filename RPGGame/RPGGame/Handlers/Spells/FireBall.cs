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
            renderer.Sprite.Texture = Engine.Instance.Content.Load<Texture2D>("fireball");

            renderer.Sprite.CenterOrigin();
            renderer.Sprite.SourceFill();

            Cleanup();
        }
        private void Controller_OnEndCast(bool interrupted)
        {
            if (interrupted) Cleanup();
        }
        #endregion

        private void Cleanup()
        {
            var controller              = Owner.FirstComponentOfType<ICharacterController>().RangedDamageController;

            controller.CastSuccessful   -= Controller_CastSuccessful;
            controller.OnEndCast        -= Controller_OnEndCast;
        }

        protected override void OnUse()
        {
            var controller              = Owner.FirstComponentOfType<ICharacterController>().RangedDamageController;

            controller.CastSuccessful   += Controller_CastSuccessful;
            controller.OnEndCast        += Controller_OnEndCast;

            controller.BeginCast(Spell, PowerSource.SpellPower, 1.25f, 10);
        }
        
        protected override void OnHit()
        {
            renderer.Destroy();

            renderer = null;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (Sensor != null)
            {
                var rot = (float)Math.Atan2(target.Position.Y - renderer.Sprite.Position.Y, target.Position.X - renderer.Sprite.Position.X);

                var dir = (target.Position - ConvertUnits.ToDisplayUnits(Sensor.Position));
                dir.Normalize();

                renderer.Sprite.Rotation    = rot;
                Sensor.LinearVelocity       = dir * MissileVelocity;
                renderer.Sprite.Position    = ConvertUnits.ToDisplayUnits(Sensor.Position);
            }
        }

        public override SpellHandler Clone()
        {
            return new Fireball();
        }
    }
}
