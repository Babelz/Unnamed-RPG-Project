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

namespace vRPGEngine.Handlers.NPC
{
    public sealed class CritterHandler : NPCHandler
    {
        #region Fields
        private Vector2 goal;
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
            Owner.FirstComponentOfType<BoxCollider>().LinearVelocity = Vector2.Zero;
        }

        public override void Initialize(Vector2 position, int level, float maxDist, Vector2[] area, Vector2 spawnLocation, Vector2 spawnBounds)
        {
            base.Initialize(position, level, maxDist, area, spawnLocation, spawnBounds);

            min = spawnLocation;
            max = spawnLocation + spawnBounds;

            goal = vRPGRandom.NextVector2(min, max);
        }

        public override void IdleUpdate(GameTime gameTime)
        {
            SharedUpdate(gameTime);

            var collider = Owner.FirstComponentOfType<BoxCollider>();
            var position = collider.DisplayPosition;

            if (Vector2.Distance(position, goal) <= 15)
            {
                if (idleElapsed >= 6500)
                {
                    goal = vRPGRandom.NextVector2(min, max);

                    idleElapsed = 0;

                    return;
                }

                idleElapsed += gameTime.ElapsedGameTime.Milliseconds;

                return;
            }
            
            var dir = goal - position;
            dir = Vector2.Normalize(dir);

            collider.LinearVelocity = 0.45f * dir;
        }
        public override bool CombatUpdate(GameTime gameTime, List<SpellHandler> spells)
        {
            SharedUpdate(gameTime);

            return true;
        }

        public override object Clone()
        {
            return new CritterHandler();
        }
    }
}
