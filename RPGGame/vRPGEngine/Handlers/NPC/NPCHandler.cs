using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vRPGContent.Data.Characters;
using vRPGContent.Data.Spells;
using vRPGEngine.ECS;
using vRPGEngine.ECS.Components;
using vRPGEngine.Handlers.Spells;

namespace vRPGEngine.Handlers.NPC
{
    public abstract class NPCHandler : ICloneable
    {
        #region Fields
        private NPCData data;

        private Vector2[] area;

        private float maxDist;
        #endregion

        #region Properties
        public NPCData Data
        {
            get
            {
                return data;
            }
        }
        public Entity Owner
        {
            get;
            set;
        }
        #endregion

        public NPCHandler()
        {
        }

        protected bool InArea()
        {
            var position = Owner.FirstComponentOfType<Transform>().Position;

            foreach (var vertex in area)
            {
                var distance = Vector2.Distance(vertex, position);

                if (distance <= maxDist) return true;
            }

            return false;
        }

        public virtual void Initialize(NPCData data, Vector2 position, int level, float maxDist, Vector2[] area, Vector2 spawnLocation, Vector2 spawnBounds)
        {
            // Copy.
            this.data = new NPCData(data);

            // Set position.
            var collider                  = Owner.FirstComponentOfType<BoxCollider>();
            collider.SimulationPosition   = position;

            this.maxDist = maxDist;
            this.area    = area;

            if (data.Level != level)
            { 
                var currentLevel = Data.Level;

                // TODO: fill rest
                var staminaPerLevel     = Data.Stamina / currentLevel;
                var intellectPerLevel   = Data.Intellect / currentLevel;
                var endurancePerLevel   = Data.Endurance / currentLevel;
                var strenghtPerLevel    = Data.Strength / currentLevel;
                var agilityPerLevel     = data.Agility / currentLevel;
                var meleePowerPerLevel  = Data.PureMeleePower / currentLevel;
                var spellPowerPerLevel  = Data.PureSpellPower / currentLevel;
                var critChancePerLevel  = Data.CriticalHitPercent / currentLevel;

                Data.Level              = level;
                Data.Stamina            += staminaPerLevel * level;
                Data.Intellect          += intellectPerLevel * level;
                Data.Endurance          += endurancePerLevel * level;
                Data.PureMeleePower     += meleePowerPerLevel * level;
                Data.PureSpellPower     += spellPowerPerLevel * level;
                Data.CriticalHitPercent += critChancePerLevel * level;
            }
        }

        public virtual void EnterCombat()
        {
        }
        public virtual void LeaveCombat()
        {
        }

        /// <summary>
        /// Do combat updates, return true if the NPC 
        /// was not inactive during this update call.
        /// Return false if the NPC was inactive during
        /// this update call.
        /// </summary>
        /// <returns>active/inactive flag</returns>
        public virtual bool CombatUpdate(GameTime gameTime, List<SpellHandler> spells)
        {
            return false;
        }
       
        /// <summary>
        /// Called when the NPC is not in combat.
        /// </summary>
        public virtual void IdleUpdate(GameTime gameTime)
        {
        }

        /// <summary>
        /// Called usually once when the NPC dies.
        /// </summary>
        public virtual void Die()
        {
        }

        public virtual void Decay()
        {
        }

        public abstract object Clone();
    }
}
