using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vRPGContent.Data.Enums;
using vRPGContent.Data.Items;
using vRPGEngine.Attributes.Spells;

namespace vRPGEngine.Attributes
{
    public class SwingBuff
    {
        /// <summary>
        /// How many percents of the total weapon
        /// damage will be used for this swing.
        /// </summary>
        public readonly float WeaponDamagePercent;

        /// <summary>
        /// How many swings this swing buff will last.
        /// </summary>
        public readonly int SwingsCount;
        
        /// <summary>
        /// The buff that triggeres this swing buff.
        /// </summary>
        public readonly Buff Buff;

        /// <summary>
        /// How many swings have been buffed.
        /// </summary>
        public int Count;

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
        private readonly List<SwingBuff> swingBuffs;

        private EquipmentContainer equipments;
        
        private Weapon[] weapons;
        private int[] timers;
        private int count;

        private List<int> results;
        #endregion

        #region Properties
        public bool InCombat
        {
            get;
            private set;
        }
        #endregion

        public MeleeDamageController(EquipmentContainer equipments)
        {
            Debug.Assert(equipments != null);

            this.equipments = equipments;

            swingBuffs  = new List<SwingBuff>();
            weapons     = new Weapon[2];
            timers      = new int[2];
        }

        public void Buff(float weaponDamagePercent, int swingsCount, Buff buff)
        {
            swingBuffs.Add(new SwingBuff(weaponDamagePercent, swingsCount, buff));
        }

        public void EnterCombat()
        {
            if (InCombat) return;

            InCombat        = true;
            timers[0]       = 0;
            timers[1]       = 0;
            count           = 0;

            if (equipments.MainHand != null) weapons[count++] = equipments.MainHand;
            if (equipments.OffHand != null)
            {
                var isOffHand = ((int)(equipments.OffHand.WeaponType & WeaponType.OffHand)) == 0;
                var isShield = ((int)(equipments.OffHand.WeaponType & WeaponType.Shield)) == 0;

                if (!(isOffHand || isShield)) weapons[count] = equipments.OffHand; 
            }
        }

        public void Tick(GameTime gameTime)
        {
            if (!InCombat) return;

            // TODO: fill.
            for (int i = 0; i < count; i++)
            {
            }
        }

        public void ExitCombat()
        {
            if (!InCombat) return;

            InCombat = false;

            weapons.Clear();
        }
    }
}
