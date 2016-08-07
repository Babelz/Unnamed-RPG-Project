using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vRPGEngine.Attributes.Spells;

namespace vRPGEngine.Attributes
{
    public struct SwingBuff
    {
        /// <summary>
        /// How many percents of the total weapon
        /// damage will be used for this swing.
        /// </summary>
        public float WeaponDamagePercent;

        /// <summary>
        /// How many swings this swing buff will last.
        /// </summary>
        public int SwingsCount;
        
        /// <summary>
        /// The buff that triggeres this swing buff.
        /// </summary>
        public Buff Buff;

        public SwingBuff(float weaponDamagePercent, int swingsCount, Buff buff)
        {
            WeaponDamagePercent = weaponDamagePercent;
            SwingsCount         = swingsCount;
            Buff                = buff;
        }
    }

    public sealed class MeleeDamageController
    {
        #region Fields
        private readonly Stack<SwingBuff> swingBuffs;

        private SwingBuff currentBuff;
        private int buffSwingsCount;
        
        private int swingTimer;
        #endregion

        public MeleeDamageController()
        {
        }

        public void Buff(float weaponDamagePercent, int swingsCount, Buff buff)
        {
            swingBuffs.Push(new SwingBuff(weaponDamagePercent, swingsCount, buff));
        }
        
        public 
    }
}
