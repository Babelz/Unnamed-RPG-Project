using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
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
using vRPGEngine.Graphics;
using vRPGEngine.Handlers.Spells;
using vRPGEngine.HUD.Controls;

namespace vRPGEngine.HUD
{
    public sealed class HUDManager : SystemManager<HUDManager>
    {
        #region Constants
        private const float IconSize        = 0.05f;
        private const int BuffIconColumns   = 10;
        private const int BuffIconRows      = 3;
        #endregion

        #region Fields
        private readonly List<Icon> buffIcons;

        private Panel root;

        private PlayerCharacterController controller;
        private Entity player;
        #endregion

        private HUDManager()
            : base()
        {
            buffIcons = new List<Icon>();
        }

        protected override void OnActivate()
        {
            root            = new Panel();
            root.Element    = null;

            HUDRenderer.Instance.Root = root;
        }
        protected override void OnSuspend()
        {
            HUDRenderer.Instance.Root = null;
        }
        
        public void Initialize(Entity player)
        {
            Debug.Assert(player != null);
            Debug.Assert(controller != null);

            this.player = player;

            controller  = player.FirstComponentOfType<PlayerCharacterController>();
            
            controller.Buffs.BuffAdded   += Buffs_BuffAdded;
            controller.Buffs.BuffRemoved += Buffs_BuffRemoved;

            // Create bottom left action bar.
            var bottomLeftActionBarButtons = new List<BindButton>();

            var offset              = HUDRenderer.Instance.CanvasSize * IconSize * 0.25f;
            var bindingsPosition    = new Vector2(offset.X, HUDRenderer.Instance.CanvasSize.Y - offset.Y);
            var key                 = (int)Keys.D1;
            
            for (int i = 0; i < 8; i++)
            {
                var bindButton          = new BindButton();
                bindButton.Keys         = (Keys)key++;
                bindButton.Size         = new Vector2(IconSize);

                var position = bindingsPosition;
                position.X += bindButton.DisplaySize.X;
                position.Y -= bindButton.DisplaySize.Y;

                bindButton.Position = position;

                bindingsPosition.X -= offset.X - bindButton.DisplaySize.X;

                bottomLeftActionBarButtons.Add(bindButton);
            }

            var buttonIndex = 0;

            foreach (var spell in controller.Spells) bottomLeftActionBarButtons[buttonIndex++].Content = spell;

            foreach (var button in bottomLeftActionBarButtons) root.Add(button);
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

        private SelfBuffSpellHandler GetHandler(Buff buff)
        {
            return player.FirstComponentOfType<PlayerCharacterController>().Spells.FirstOrDefault(h => h.Spell.ID == buff.FromSpell.ID) as SelfBuffSpellHandler;
        }
        
        private void SortIcons()
        {
            if (buffIcons.Count == 0) return;

            var offset      = HUDRenderer.Instance.CanvasSize * IconSize * 0.25f;

            Vector2 positon = HUDRenderer.Instance.CanvasSize - (HUDRenderer.Instance.CanvasSize * IconSize);
            positon.X       -= offset.X;
            positon.Y       = offset.Y;

            var iconIndex   = 0;
            
            for (int i = 0; i < BuffIconRows; i++)
            {
                for (int j = 0; j < BuffIconColumns; j++)
                {
                    var icon = buffIcons[iconIndex++];

                    icon.Position = new Vector2(positon.X - offset.X, positon.Y + offset.Y);

                    if (iconIndex == buffIcons.Count) return;

                    positon.X -= offset.X - icon.DisplaySize.X;
                }

                positon.Y += offset.Y + HUDRenderer.Instance.CanvasSize.Y * IconSize;
            }
        }

        private void AddIcon(Buff buff)
        {
            var icon     = new Icon();
            icon.Size    = new Vector2(IconSize);
            icon.Content = GetHandler(buff);
        
            HUDRenderer.Instance.Root.Add(icon);

            buffIcons.Add(icon);
        }
        private void RemoveIcon(Buff buff)
        {
            var icon = buffIcons.First(i => ReferenceEquals(i.Content, GetHandler(buff)));

            HUDRenderer.Instance.Root.Remove(icon);

            buffIcons.Remove(icon);
        }
    }
}
