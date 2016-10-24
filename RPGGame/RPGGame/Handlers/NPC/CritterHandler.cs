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
using vRPGEngine.Core;
using vRPGEngine.Handlers.NPC;
using vRPGEngine;
using vRPGEngine.Extensions;

namespace RPGGame.Handlers.NPC
{
    public sealed class CritterHandler : NPCHandler
    {
        #region Constants
        private static readonly Vector2 Velocity = new Vector2(0.45f);

        private const int IdleMin = 150;
        private const int IdleMax = 350;
        #endregion

        #region Fields
        private Vector2 goal;

        private int nextIdleTime;
        private int idleElapsed;

        private Vector2 min;
        private Vector2 max;

        private readonly SampleList<Vector2> positionSamples;
        #endregion

        public CritterHandler() 
            : base()
        {
            positionSamples = new SampleList<Vector2>(8);
        }

        public override void Initialize(Entity owner, NPCData data, int level, float maxDist, Vector2? position = null, Rectf? area = null)
        {
            base.Initialize(owner, data, level, maxDist, position, area);

            min          = area.Value.TopLeft;
            max          = area.Value.BottomRight;

            goal         = vRPGRandom.NextVector2(min, max);
            nextIdleTime = vRPGRandom.NextInt(IdleMin, IdleMax);

            owner.FirstComponentOfType<ICharacterController>().Statuses.HealthChanged += Statuses_HealthChanged;
        }

        #region Event handlers
        private void Statuses_HealthChanged(Statuses sender, int newValue, int oldValue)
        {
            idleElapsed = 0;
        }
        #endregion

        private void ChangeDirection()
        {
            goal         = vRPGRandom.NextVector2(min, max);
            nextIdleTime = vRPGRandom.NextInt(IdleMin, IdleMax);
            idleElapsed  = 0;
        }

        public override void IdleUpdate(GameTime gameTime)
        {
            var renderer    = Owner.FirstComponentOfType<SpriteRenderer>();
            var collider    = Owner.FirstComponentOfType<Collider>();

            var dir = goal - collider.DisplayPosition;
            dir.Normalize();

            collider.LinearVelocity = dir * Velocity;

            if (Vector2.Distance(goal, collider.DisplayPosition) <= 15.0f) ChangeDirection();

            if (dir.X > 0) renderer.Sprite.Effects = SpriteEffects.FlipHorizontally;
            else           renderer.Sprite.Effects = SpriteEffects.None;

            if (positionSamples.Values.Count(p => VectorExtensions.AlmostEqual(p, positionSamples.First())) == positionSamples.MaxSamples) ChangeDirection();

            positionSamples.Add(collider.DisplayPosition);
        }

        public override void Die()
        {
            var renderer           = Owner.FirstComponentOfType<SpriteRenderer>();
            renderer.Sprite.Source = new Rectangle(64, 0, 32, 32);

            LeaveCombat();
        }

        public override bool CombatUpdate(GameTime gameTime, List<SpellHandler> spells)
        {
            Owner.FirstComponentOfType<Collider>().LinearVelocity = Vector2.Zero;

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

        public override NPCHandler Clone()
        {
            return new CritterHandler();
        }
    }
}
