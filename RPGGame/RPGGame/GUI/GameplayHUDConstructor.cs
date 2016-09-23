﻿using Microsoft.Xna.Framework;
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
using vRPGEngine.HUD;
using vRPGEngine.HUD.Controls;
using vRPGEngine.HUD.Elements;

namespace RPGGame.GUI
{
    public sealed class GameplayHUDConstructor : HUDConstructor
    {
        #region Fields
        private readonly List<Icon> buffIcons;
        
        private PlayerCharacterController controller;
        private Entity player;
        #endregion

        public GameplayHUDConstructor()
        {
            buffIcons = new List<Icon>();
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
            return player.FirstComponentOfType<PlayerCharacterController>().Spells
                         .FirstOrDefault(h => h.Spell.ID == buff.FromSpell.ID) as SelfBuffSpellHandler;
        }

        private void SortIcons()
        {
            if (buffIcons.Count == 0) return;

            var offset = HUDRenderer.Instance.CanvasSize * HUDConstants.IconSize * 0.25f;

            Vector2 positon = HUDRenderer.Instance.CanvasSize - (HUDRenderer.Instance.CanvasSize * HUDConstants.IconSize);
            positon.X       -= offset.X;
            positon.Y       = offset.Y;

            var iconIndex = 0;

            for (int i = 0; i < HUDConstants.BuffIconRows; i++)
            {
                for (int j = 0; j < HUDConstants.BuffIconColumns; j++)
                {
                    var icon = buffIcons[iconIndex++];

                    icon.Position = new Vector2(positon.X - offset.X, positon.Y + offset.Y);

                    if (iconIndex == buffIcons.Count) return;

                    positon.X -= offset.X - icon.DisplaySize.X;
                }

                positon.Y += offset.Y + HUDRenderer.Instance.CanvasSize.Y * HUDConstants.IconSize.Y;
            }
        }
        
        private void ConstructHUD()
        {
            player      = EntityManager.Instance.Entitites.First(e => e.Tagged("player"));
            
            Debug.Assert(player != null);

            controller  = player.FirstComponentOfType<PlayerCharacterController>();
            View        = player.FirstComponentOfType<Camera>().View;

            Debug.Assert(controller != null);
            Debug.Assert(View != null);

            controller.Buffs.BuffAdded   += Buffs_BuffAdded;
            controller.Buffs.BuffRemoved += Buffs_BuffRemoved;

            Root = new Panel();

            // Create bottom left action bar.
            var bottomLeftActionBarButtons = new List<BindButton>();

            var startPosition       = HUDRenderer.Instance.CanvasSize * HUDConstants.IconSize * 0.25f;
            var bindingsPosition    = new Vector2(startPosition.X, HUDRenderer.Instance.CanvasSize.Y - startPosition.Y);
            var key                 = (int)Keys.D1;

            for (int i = 0; i < 8; i++)
            {
                var bindButton  = new BindButton();
                bindButton.Keys = (Keys)key++;
                bindButton.Size = HUDConstants.IconSize;

                var position = bindingsPosition;
                position.X   += bindButton.DisplaySize.X;
                position.Y   -= bindButton.DisplaySize.Y;

                bindButton.Position = position;
                bindButton.Element  = new IconElement(new SpellIconHandler());

                bindingsPosition.X  += bindButton.DisplaySize.X;

                bottomLeftActionBarButtons.Add(bindButton);
            }

            var buttonIndex = 0;

            foreach (var spell in controller.Spells) bottomLeftActionBarButtons[buttonIndex++].Content = spell;

            foreach (var button in bottomLeftActionBarButtons) Root.Add(button);
        }
        private void ConstructSubsystems()
        {
            Subsystems = new List<HUDSubsystem>()
            {
                new CombatTextManager()
            };
        }

        private void AddIcon(Buff buff)
        {
            var icon        = new Icon();
            icon.Size       = HUDConstants.IconSize;
            icon.Element    = new IconElement(new BuffIconHandler());
            icon.Content    = GetHandler(buff);

            Root.Add(icon);

            buffIcons.Add(icon);
        }
        private void RemoveIcon(Buff buff)
        {
            var icon = buffIcons.First(i => ReferenceEquals(i.Content, GetHandler(buff)));

            Root.Remove(icon);

            buffIcons.Remove(icon);
        }

        public override void Construct()
        {
            ConstructHUD();
            ConstructSubsystems();
        }
    }
}