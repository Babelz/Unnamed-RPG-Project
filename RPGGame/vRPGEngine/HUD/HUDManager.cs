using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vRPGEngine.Attributes;
using vRPGEngine.Attributes.Spells;
using vRPGEngine.ECS;
using vRPGEngine.ECS.Components;
using vRPGEngine.HUD.Controls;

namespace vRPGEngine.HUD
{
    public sealed class HUDManager : SystemManager<HUDManager>
    {
        #region Constants
        private const int BuffIconColumns = 10;
        private const int BuffIconRows    = 3;
        #endregion

        #region Fields
        private readonly List<Icon> buffIcons;

        private PlayerCharacterController controller;
        private Entity player;
        #endregion

        private HUDManager()
            : base()
        {
        }

        public void Initialize(Entity player)
        {
            Debug.Assert(player != null);
            Debug.Assert(controller != null);

            this.player = player;

            controller  = player.FirstComponentOfType<PlayerCharacterController>();

            controller.Buffs.BuffAdded   += Buffs_BuffAdded;
            controller.Buffs.BuffRemoved += Buffs_BuffRemoved;
        }

        #region Event handlers
        private void Buffs_BuffRemoved(BuffContainer sender, Buff buff)
        {
            RemoveIcon(buff);

            SortIcons();
        }
        private void Buffs_BuffAdded(BuffContainer sender, Buff buff)
        {
            AddIcon(buff);

            SortIcons();
        }
        #endregion
        
        private void SortIcons()
        {
        }

        private void AddIcon(Buff buff)
        {
        }
        private void RemoveIcon(Buff buff)
        {
        }
    }
}
