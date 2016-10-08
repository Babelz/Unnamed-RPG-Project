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
        #region Constant fields
        /// <summary>
        /// To make spell damage have a small variance.
        /// </summary>
        public const float Variance = 0.1f;
        #endregion

        #region Fields
        private float percentageOfSource;
        private int baseDamage;

        private Specialization specialization;

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
        public event RangedDamageControllerCastEventHandler CastSuccessful;

        public event RangedDamageControllerBeginCastEventHandler OnBeginCast;
        public event RangedDamageControllerEndCastEventHandler OnEndCast;
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

        private void EndCast(bool interrupted)
        {
            spell           = null;

            OnEndCast?.Invoke(interrupted);
        }

        public void Initialize(Specialization specialization)
        {
            this.specialization = specialization;
        }

        public void BeginCast(Spell spell, PowerSource source, float percentageOfSource, int baseDamage)
        {
            Debug.Assert(specialization != null);
            Debug.Assert(spell != null);

            this.percentageOfSource = percentageOfSource;
            this.baseDamage         = baseDamage;

            EndCast();
            
            this.spell          = spell;
            this.source         = source;

            castTimer = 0;

            OnBeginCast?.Invoke();
        }
        public void EndCast()
        {
            EndCast(Casting);
        }

        public void GenerateCast(ref RangedDamageResults results, PowerSource source, float percentageOfSource, int baseDamage)
        {
            var critical            = specialization.TotalCriticalHitPercent() >= vRPGRandom.NextFloat();
            var power               = (int)(GetPower(source, specialization) * percentageOfSource);
            var powerVariance       = (int)(power * Variance);
            var damage              = baseDamage + vRPGRandom.NextInt(power - powerVariance, power + powerVariance);

            damage                  = critical ? (int)(damage * specialization.CriticalDamagePercent()) : damage;

            results.Damage          = damage;
            results.Critical        = critical;
        }

        public void Update(GameTime gameTime)
        {
            if (!Casting) return;

            castTimer += gameTime.ElapsedGameTime.Milliseconds;
            
            if (castTimer >= spell.CastTime)
            {
                results = RangedDamageResults.Empty;

                GenerateCast(ref results, source, percentageOfSource, baseDamage);
                
                CastSuccessful?.Invoke(ref results);
                
                EndCast(false);
            } 
        }

        public delegate void RangedDamageControllerCastEventHandler(ref RangedDamageResults results);
        public delegate void RangedDamageControllerBeginCastEventHandler();
        public delegate void RangedDamageControllerEndCastEventHandler(bool interrupted);

    }
}
