using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vRPGContent.Data.Items;
using vRPGContent.Data.Items.Enums;
using vRPGContent.Data.Spells;
using vRPGEngine.Attributes.Spells;
using vRPGEngine.Specializations;

namespace vRPGEngine.Attributes
{  
    public struct MeleeSwingResults
    {
        public int Damage;

        public bool Critical;

        public Weapon Weapon;
    }

    public sealed class MeleeDamageController
    {
        #region Fields
        private readonly EquipmentContainer equipments;
        private readonly Specialization specialization;
        
        private List<MeleeSwingResults> results;
        
        private Weapon[] weapons;
        private int[] timers;
        private int count;
        #endregion

        #region Events
        public event MeleeSwingEventHandler OnSwing;
        #endregion

        #region Properties
        public bool InCombat
        {
            get;
            private set;
        }
        #endregion

        public MeleeDamageController(EquipmentContainer equipments, Specialization specialization)
        {
            Debug.Assert(equipments != null);

            this.equipments     = equipments;
            this.specialization = specialization;

            results     = new List<MeleeSwingResults>();
            weapons     = new Weapon[2];
            timers      = new int[2];
        }

        public void EnterCombat()
        {
            if (InCombat) return;

            InCombat        = true;
            count           = 0;

            if (equipments.MainHand != null)
            {
                timers[count]       = equipments.MainHand.SwingTimer;
                weapons[count++]    = equipments.MainHand;
            }
            if (equipments.OffHand != null)
            {
                var isOffHand = ((int)(equipments.OffHand.WeaponType & WeaponType.OffHand)) == 0;
                var isShield = ((int)(equipments.OffHand.WeaponType & WeaponType.Shield)) == 0;

                if (!(isOffHand || isShield))
                {
                    timers[count]   = equipments.OffHand.SwingTimer;
                    weapons[count]  = equipments.OffHand;
                } 
            }
        }

        public void Tick(GameTime gameTime)
        {
            if (!InCombat) return;

            // TODO: fill.
            for (int i = 0; i < count; i++)
            {
                var weapon      = weapons[i];

                if (timers[i] >= weapon.SwingTimer)
                {
                    timers[i] -= weapon.SwingTimer;

                    if (timers[i] < 0) timers[i] = 0;
                }
                else
                {
                    timers[i] += gameTime.ElapsedGameTime.Milliseconds;

                    continue;
                }
                
                var critical    = specialization.TotalCriticalHitPercent() <= vRPGRandom.NextFloat();
                var damage      = vRPGRandom.NextInt(weapon.BaseDamageMin, weapon.BaseDamageMax);

                damage = critical ? (int)(damage * specialization.CriticalDamagePercent()) : damage;

                var swing       = new MeleeSwingResults();
                swing.Damage    = damage + (int)(damage * (specialization.MeleeDamageModifierPercent() + specialization.DamageModifierPercent()));
                swing.Critical  = critical;
                swing.Weapon    = weapon;

                results.Add(swing);
            }
        }

        public IEnumerable<MeleeSwingResults> Results()
        {
            for (int i = 0; i < results.Count; i++)
            {
                var swing = results[i];

                OnSwing?.Invoke(ref swing);

                yield return swing;
            }

            results.Clear();
        }

        public void ExitCombat()
        {
            if (!InCombat) return;

            InCombat = false;
            count    = 0;
        }

        public delegate void MeleeSwingEventHandler(ref MeleeSwingResults swing);
    }
}
