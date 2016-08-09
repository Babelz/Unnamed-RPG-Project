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
            set
            {
                data = new NPCData(value);
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

        public virtual void Initialize(Vector2 position, int level, float maxDist, Vector2[] area, Vector2 spawnLocation, Vector2 spawnBounds)
        {
            var collider        = Owner.FirstComponentOfType<BoxCollider>();
            collider.SimulationPosition   = position;

            this.maxDist = maxDist;
            this.area    = area;

            var currentLevel = Data.Level;

            var healthPerLevel      = Data.Health / currentLevel;
            var manaPerLevel        = Data.Mana / currentLevel;
            var focusPerLevel       = Data.Focus / currentLevel;
            var meleePowerPerLevel  = Data.MeleePower / currentLevel;
            var spellPowerPerLevel  = Data.SpellPower / currentLevel;
            var critChancePerLevel  = Data.CritChance / currentLevel;

            Data.Level      = level;
            Data.Health     += healthPerLevel * level;
            Data.Mana       += manaPerLevel * level;
            Data.Focus      += focusPerLevel * level;
            Data.MeleePower += meleePowerPerLevel * level;
            Data.SpellPower += spellPowerPerLevel * level;
            Data.CritChance += critChancePerLevel * level;
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
        /// Called usually once when the NPC dies.
        /// </summary>
        public virtual void Die(GameTime gameTime)
        {
        }

        /// <summary>
        /// Called when the NPC is not in combat.
        /// </summary>
        public virtual void IdleUpdate(GameTime gameTime)
        {
        }

        public abstract object Clone();
    }
}
