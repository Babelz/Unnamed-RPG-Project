using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vRPGContent.Data.Characters;

namespace vRPGEngine.ECS.Components
{
    public sealed class SpawnArea : Component<SpawnArea>
    {
        #region Constants
        public const int DefaultSpawnTime   = 1000;
        public const int DefaultMaxNPCs     = 5;
        public const float DefaultMaxDist   = 20.0f;
        #endregion

        #region Fields
        private readonly List<Entity> npcs;

        private Vector2 position;
        private Vector2 bounds;

        private Vector2[] area;
        private NPCData data;

        private int minLevel;
        private int maxLevel;
        private int maxNPCs;

        private float maxDist;

        private int spawnTime;
        private int elasped;
        #endregion
        
        public SpawnArea()
            : base()
        {
            npcs = new List<Entity>();
        }

        private Entity Spawn()
        {
            var npc = EntityBuilder.Instance.Create(data.Name.ToLower());

            if (npc == null)
            {
                Logger.Instance.LogFunctionWarning(string.Format("could not create npc with id {0}, with prefab name {1}", data.ID, data.Name.ToLower()));

                return null;
            }

            var controller  = npc.FirstComponentOfType<NPCController>();
            var level       = vRPGRandom.NextInt(minLevel, maxLevel);
            var position    = new Vector2(vRPGRandom.NextFloat(this.position.X, this.position.X + bounds.X),
                                          vRPGRandom.NextFloat(this.position.Y, this.position.Y + bounds.Y));

            controller.Handler.Initialize(position, level, maxDist, area, position, bounds);

            controller.OnDecayed += Controller_OnDecayed;
            controller.OnDeath   += Controller_OnDeath;

            return npc;
        }

        private void Controller_OnDeath(NPCController controller)
        {
            controller.Handler.Die();
        }

        private void Controller_OnDecayed(NPCController controller)
        {
            controller.Handler.Decay();

            npcs.Remove(controller.Owner);

            controller.Owner.Destroy();
        }

        public void Initialize(NPCData data, Vector2 position, Vector2 bounds, int minLevel, int maxLevel, int maxNPCs, int spawnTime, float maxDist)
        {
            Debug.Assert(data != null);
            
            this.data           = data;
            this.position       = position;
            this.bounds         = bounds;
            this.minLevel       = minLevel == 0 ? data.Level : minLevel;
            this.maxLevel       = maxLevel == 0 ? data.Level : maxLevel;
            this.maxNPCs        = maxNPCs;
            this.spawnTime      = spawnTime;
            this.maxDist        = maxDist;

            area    = new Vector2[4];
            area[0] = position;                                         // tl
            area[1] = new Vector2(position.X + bounds.X, position.Y);   // tr
            area[2] = new Vector2(position.X, position.Y + bounds.Y);   // bl
            area[3] = position + bounds;                                // br
        }

        public void Update(GameTime gameTime)
        {
            if (npcs.Count >= maxNPCs) return;

            elasped += gameTime.ElapsedGameTime.Milliseconds;

            if (elasped < spawnTime) return;

            elasped = 0;

            var npc = Spawn();

            if (npc != null) npcs.Add(npc);
        }
    }
}
