using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vRPGContent.Data.Characters;

namespace vRPGEngine.ECS.Components
{
    public sealed class SpawnArea : Component<SpawnArea>
    {
        #region Fields
        private readonly List<Entity> npcs;
        
        private Vector2[] vertices;
        private NPCData data;

        private int maxNPCCount;
        #endregion

        public SpawnArea()
            : base()
        {
            npcs = new List<Entity>();
        }

        private Entity CreateNPC()
        {
            var npc = EntityBuilder.Instance.Create(data.Name.ToLower());

            if (npc == null) return null;

            var controller = npc.FirstComponentOfType<NPCController>();

            controller.OnDecayed += Controller_OnDecayed;

            return npc;
        }

        private void Controller_OnDecayed(NPCController controller)
        {
        }

        public void Initialize(NPCData npcData, int minLevel, int maxLevel, float maxDistance)
        {
        }

        public void Spawn()
        {
            if (npcs.Count >= maxNPCCount) return;

            var npc = CreateNPC();

            if (npc != null) npcs.Add(npc);
        }
    }
}
