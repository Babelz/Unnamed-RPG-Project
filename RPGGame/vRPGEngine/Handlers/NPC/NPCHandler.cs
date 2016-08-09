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
using vRPGEngine.Handlers.Spells;

namespace vRPGEngine.Handlers.NPC
{
    public abstract class NPCHandler 
    {
        #region Properties
        public string Name
        {
            get;
            private set;
        }

        public NPCData Data
        {
            get;
            private set;
        }

        public Entity Owner
        {
            get;
            set;
        }
        #endregion

        protected NPCHandler(string name, NPCData data)
        {
            Debug.Assert(!string.IsNullOrEmpty(name));
            Debug.Assert(data != null);
            
            Name = name;
            Data = new NPCData(data);
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
        public bool CombatUpdate(GameTime gameTime, List<SpellHandler> spells)
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
    }
}
