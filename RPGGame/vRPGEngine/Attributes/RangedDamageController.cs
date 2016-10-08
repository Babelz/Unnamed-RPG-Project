using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vRPGContent.Data.Spells;
using vRPGEngine.Core;
using vRPGEngine.Specializations;

namespace vRPGEngine.Attributes
{
    public struct RangedDamageResults
    {
        #region Empty results 
        public static readonly RangedDamageResults Empty = new RangedDamageResults() { Damage = 0, Critical = false };
        #endregion

        #region Fields
        public int Damage;

        public bool Critical;
        #endregion
    }

    public enum PowerSource : int
    {
        // Intellect and spell power sources.
        SpellPower,
        Intellect,
        IntellectWithSpellpower,

        // Attack power and agility based sources.
        AttackPower,
        Agility,
        AttackPowerWithAgility
    }

    /// <summary>
    /// Class that is responsible of handling ranged spell
    /// handling.
    /// </summary>
    public sealed class RangedDamageController
    {
        #region Fields
        /// <summary>
        /// Holds the spell casting results of the last
        /// successful cast.
        /// </summary>
        private RangedDamageResults results;

        private Spell spell;
        private PowerSource source;

        private int castTimer;
        #endregion

        #region Properties
        public bool Casting
        {
            get
            {
                return spell != null;
            }
        }
        public RangedDamageResults Results
        {
            get
            {
                return results;
            }
        }
        #endregion

        #region Events
        public event RangedDamageControllerEventHandler CastSuccessful;
        #endregion
        
        public RangedDamageController()
        {
        }

        private int GetPower(PowerSource source, Specialization specialization)
        {
            switch (source)
            {
                case PowerSource.SpellPower:                return specialization.TotalSpellPower();
                case PowerSource.AttackPower:               return specialization.TotalAttackPower();
                case PowerSource.Agility:                   return specialization.TotalAgility();
                case PowerSource.AttackPowerWithAgility:    return specialization.TotalAttackPower() + specialization.TotalAgility();
                default:                                    throw new vRPGEngineException("unsupported power source!");
            }
        }

        public void BeginCast(Spell spell, PowerSource source)
        {
            Debug.Assert(spell != null);

            this.spell  = spell;
            this.source = source;

            castTimer   = 0;
        }
        public void EndCast()
        {
            spell = null;
        }

        public void GenerateCast(ref RangedDamageResults results, PowerSource source)
        {
        }

        public void Update(GameTime gameTime)
        {
            if (!Casting) return;

            castTimer -= gameTime.ElapsedGameTime.Milliseconds;
            
            if (castTimer >= spell.CastTime)
            {
                results = RangedDamageResults.Empty;

                GenerateCast(ref results, source);

                CastSuccessful?.Invoke(ref results);

                EndCast();
            } 
        }

        public delegate void RangedDamageControllerEventHandler(ref RangedDamageResults results);
    }
}
