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
    
    public enum Hand
    {
        Main    = 0,
        Off     = 1
    }

    public sealed class MeleeDamageController
    {
        #region Fields
        private EquipmentContainer equipments;
        private Specialization specialization;
        
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

        public MeleeDamageController()
        {
            results     = new List<MeleeSwingResults>();
            weapons     = new Weapon[2];
            timers      = new int[2];
        }

        private void GenerateSilentSwing(ref MeleeSwingResults swing, Weapon weapon)
        {
            var critical    = specialization.TotalCriticalHitPercent() >= vRPGRandom.NextFloat();
            var damage      = vRPGRandom.NextInt(weapon.BaseDamageMin, weapon.BaseDamageMax);

            damage          = critical ? (int)(damage * specialization.CriticalDamagePercent()) : damage;

            swing.Damage    = damage + (int)(damage * (specialization.MeleeDamageModifierPercent() + specialization.DamageModifierPercent()));
            swing.Critical  = critical;
            swing.Weapon    = weapon;
        }

        public void GenerateSwing(ref MeleeSwingResults swing, Hand hand = Hand.Main)
        {
            GenerateSilentSwing(ref swing, weapons[(int)hand]);

            OnSwing?.Invoke(ref swing);
        }

        public void GenerateMeleeAttackPowerBasedSwing(ref MeleeSwingResults swing, float percent, Hand hand = Hand.Main)
        {
            GenerateSilentSwing(ref swing, weapons[(int)hand]);

            swing.Damage += (int)(specialization.TotalMeleePower() * percent);

            OnSwing?.Invoke(ref swing);
        }

        public void Initialize(EquipmentContainer equipments, Specialization specialization)
        {
            Debug.Assert(equipments != null);
            Debug.Assert(specialization != null);
            
            this.equipments     = equipments;
            this.specialization = specialization;
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
                var isOffHand = (int)(equipments.OffHand.WeaponType & WeaponType.OffHand) == 1;
                var isShield  = (int)(equipments.OffHand.WeaponType & WeaponType.Shield) == 1;

                if (!(isOffHand || isShield))
                {
                    timers[count]   = equipments.OffHand.SwingTimer;
                    weapons[count]  = equipments.OffHand;
                } 
            }
        }

        public void Update(GameTime gameTime)
        {
            if (!InCombat) return;

            for (int i = 0; i < count; i++)
            {
                var weapon = weapons[i];
                
                if (timers[i] >= weapon.SwingTimer)
                {
                    timers[i] -= weapon.SwingTimer;
                }
                else
                {
                    timers[i] += gameTime.ElapsedGameTime.Milliseconds;

                    continue;
                }

                if (results.Count < count)
                {
                    MeleeSwingResults swing = new MeleeSwingResults();

                    GenerateSilentSwing(ref swing, weapon);

                    results.Add(swing);
                }
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

        public void LeaveCombat()
        {
            if (!InCombat) return;

            InCombat = false;
            count    = 0;
        }

        public delegate void MeleeSwingEventHandler(ref MeleeSwingResults swing);
    }
}
