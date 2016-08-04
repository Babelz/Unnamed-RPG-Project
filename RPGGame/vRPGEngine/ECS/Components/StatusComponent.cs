using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vRPGEngine.ECS.Components
{
    /// <summary>
    /// Contains all the base stats for all entities in the world.
    /// </summary>
    public sealed class StatusComponent : Component<StatusComponent>
    {
        #region Properties
        public int Stamina
        {
            get;
            set;
        }
        public int Intellect
        {
            get;
            set;
        }
        public int Endurance
        {
            get;
            set;
        }
        public int Strength
        {
            get;
            set;
        }
        public int Agility
        {
            get;
            set;
        }
        public int Mp5
        {
            get;
            set;
        }
        public int Ep5
        {
            get;
            set;
        }
        public int Hp5
        {
            get;
            set;
        }
        public int Haste
        {
            get;
            set;
        }
        public float CriticalHitPercent
        {
            get;
            set;
        }
        public float DefenceRatingPercent
        {
            get;
            set;
        }
        public float BlockRatingPercent
        {
            get;
            set;
        }
        public float DodgeRatingPercent
        {
            get;
            set;
        }
        public float ParryRatingPercent
        {
            get;
            set;
        }
        public float MovementSpeedPercent
        {
            get;
            set;
        }
        #endregion

        public StatusComponent()
            : base()
        {
        }
    }
}
