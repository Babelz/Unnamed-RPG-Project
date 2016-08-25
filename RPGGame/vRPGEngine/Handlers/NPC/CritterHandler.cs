using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using vRPGContent.Data.Characters;
using vRPGEngine.Handlers.Spells;
using vRPGEngine.ECS.Components;
using vRPGEngine.ECS;
using Microsoft.Xna.Framework.Graphics;
using vRPGEngine.Combat;
using vRPGEngine.Attributes;

namespace vRPGEngine.Handlers.NPC
{
    public sealed class CritterHandler : NPCHandler
    {
        #region Fields
        private Vector2 goal;

        private int nextIdleTime;
        private int idleElapsed;

        private Vector2 min;
        private Vector2 max;
        #endregion

        public CritterHandler() 
            : base()
        {
        }

        private void SharedUpdate(GameTime gameTime)
        {
            Owner.FirstComponentOfType<Collider>().LinearVelocity = Vector2.Zero;
        }

        public override void Initialize(Entity owner, NPCData data, int level, float maxDist, Vector2? position = null, Rectf? area = null)
        {
            base.Initialize(owner, data, level, maxDist, position, area);

            min          = area.Value.TopLeft;
            max          = area.Value.BottomRight;

            goal         = vRPGRandom.NextVector2(min, max);
            nextIdleTime = vRPGRandom.NextInt(1500, 10000);

            owner.FirstComponentOfType<ICharacterController>().Statuses.HealthChanged += Statuses_HealthChanged;
        }

        #region Event handlers
        private void Statuses_HealthChanged(Statuses sender, int newValue, int oldValue)
        {
            idleElapsed = 0;
        }
        #endregion

        public override void IdleUpdate(GameTime gameTime)
        {
            SharedUpdate(gameTime);

            var collider = Owner.FirstComponentOfType<Collider>();
            var position = collider.DisplayPosition;

            if (Vector2.Distance(position, goal) <= 15)
            {
                if (idleElapsed >= nextIdleTime)
                {
                    goal         = vRPGRandom.NextVector2(min, max);
                    nextIdleTime = vRPGRandom.NextInt(1500, 10000);
                    idleElapsed  = 0;

                    return;
                }

                idleElapsed += gameTime.ElapsedGameTime.Milliseconds;

                return;
            }
            
            var dir = goal - position;
            dir     = Vector2.Normalize(dir);

            collider.LinearVelocity = 0.45f * dir;

            var renderer = Owner.FirstComponentOfType<SpriteRenderer>();

            if (dir.X > 0) renderer.Sprite.Effects = SpriteEffects.FlipHorizontally;
            else           renderer.Sprite.Effects = SpriteEffects.None;
        }

        public override void Die()
        {
            var renderer           = Owner.FirstComponentOfType<SpriteRenderer>();
            renderer.Sprite.Source = new Rectangle(64, 0, 32, 32);

            LeaveCombat();
        }

        public override bool CombatUpdate(GameTime gameTime, List<SpellHandler> spells)
        {
            SharedUpdate(gameTime);

            idleElapsed += gameTime.ElapsedGameTime.Milliseconds;

            return idleElapsed <= 3500;
        }

        public override void EnterCombat()
        {
            idleElapsed = 0;

            CombatManager.Instance.RegisterHostile(Owner.FirstComponentOfType<ICharacterController>());
        }
        public override void LeaveCombat()
        {
            CombatManager.Instance.UnregisterHostile(Owner.FirstComponentOfType<ICharacterController>());
        }

        public override object Clone()
        {
            return new CritterHandler();
        }
    }
}
