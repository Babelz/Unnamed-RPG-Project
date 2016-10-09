using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vRPGEngine.Attributes.Enums;
using vRPGEngine.Attributes.Spells;

namespace vRPGEngine.Attributes
{
    public delegate void BuffContainerBuffEventHandler(BuffContainer sender, Buff buff);

    public sealed class BuffContainer
    {
        #region Constants
        public const int MaxBuffs   = 12;
        public const int MaxDebuffs = 12;

        public static readonly int[] MaxValues = new int[]
        {
            MaxBuffs,
            MaxDebuffs
        };
        #endregion

        #region Fields
        private readonly List<Buff> buffs;

        private int[] counts;
        #endregion

        #region Events
        public event BuffContainerBuffEventHandler BuffAdded;
        public event BuffContainerBuffEventHandler BuffRemoved;
        #endregion

        #region Properties
        public int DebuffsCount
        {
            get
            {
                return counts[(int)BuffType.Debuff];
            }
            private set
            {
                counts[(int)BuffType.Debuff] = value;
            }
        }
        public int BuffsCount
        {
            get
            {
                return counts[(int)BuffType.Buff];
            }
            private set
            {
                counts[(int)BuffType.Buff] = value;
            }
        }

        public IEnumerable<Buff> Buffs
        {
            get
            {
                return buffs.Where(b => b.Type == BuffType.Buff);
            }
        }
        public IEnumerable<Buff> Debuffs
        {
            get
            {
                return buffs.Where(b => b.Type == BuffType.Debuff);
            }
        }
        #endregion

        public BuffContainer()
        {
            buffs  = new List<Buff>();
            counts = new int[2];
        }

        public bool Add(Buff buff)
        {
            Debug.Assert(buff != null);

            var add = counts[(int)buff.Type] + 1 <= MaxValues[(int)buff.Type];

            if (add)
            {
                buffs.Add(buff);

                counts[(int)buff.Type]++;
                
                BuffAdded?.Invoke(this, buff);   
            }

            return add;
        }

        public bool Remove(Buff buff)
        {
            Debug.Assert(buff != null);

            if (buffs.Remove(buff))
            {
                counts[(int)buff.Type]--;

                BuffRemoved?.Invoke(this, buff);
                
                return true;
            }

            return false;
        }
    }
}
