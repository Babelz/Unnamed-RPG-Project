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
using vRPGEngine.Core;
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

        private BuffSpellHandler GetHandler(Buff buff)
        {
            return player.FirstComponentOfType<PlayerCharacterController>().Spells
                         .FirstOrDefault(h => h.Spell.ID == buff.FromSpell.ID) as BuffSpellHandler;
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
            View        = player.FirstComponentOfType<Camera>()?.View;

            Debug.Assert(controller != null);
            Debug.Assert(View != null);

            Root = new Panel()
            {
                Element = null
            };

            CreateActionbars();

            CreatePlayerStatusWindow();

            CreateTargetStatusWindow();
        }

        private void CreateActionbars()
        {
            controller.Buffs.BuffAdded      += Buffs_BuffAdded;
            controller.Buffs.BuffRemoved    += Buffs_BuffRemoved;

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

                var position    = bindingsPosition;
                position.X      += bindButton.DisplaySize.X;
                position.Y      -= bindButton.DisplaySize.Y;

                bindButton.Position = position;
                bindButton.Element  = new IconElement(new SpellIconHandler());

                bindingsPosition.X += bindButton.DisplaySize.X;

                bottomLeftActionBarButtons.Add(bindButton);
            }

            var buttonIndex = 0;

            foreach (var spell in controller.Spells) bottomLeftActionBarButtons[buttonIndex++].Content = spell;

            foreach (var button in bottomLeftActionBarButtons) Root.Add(button);
        }

        private void CreatePlayerStatusWindow()
        {
            // Player status window init.
            var statusWindow        = new Panel();
            statusWindow.Sizing     = Sizing.Percents;
            statusWindow.Size       = new Vector2(0.15f);
            statusWindow.Position   = new Vector2(HUDRenderer.Instance.CanvasSize.X * 0.015f, HUDRenderer.Instance.CanvasSize.Y * 0.015f);
            statusWindow.Element    = null;

            // Target name label.
            var statusesHeader  = new Label();
            statusesHeader.Text = "Statuses";
            statusWindow.Add(statusesHeader);

            statusWindow.Position = new Vector2(statusWindow.Position.X, statusesHeader.DisplayBounds.Bottom);
            statusesHeader.Position = new Vector2(statusesHeader.Position.X, -statusesHeader.DisplayBounds.Height);

            // Health bar init.
            var healthBar       = new StatusBar();
            healthBar.SetPresentationData("HUD\\parts",
                                          StatusBarTextureSources.Compute(0, 0, 32, 32, 4, 1),
                                          StatusBarBindingsSource.Create(() => 0,
                                                                         () => controller.Specialization.TotalHealth(),
                                                                         () => controller.Statuses.Health));
            healthBar.ShowText  = true;
            healthBar.Size      = new Vector2(0.8f, 0.33f);
            healthBar.TextType  = TextType.Both;
            statusWindow.Add(healthBar);

            // Mana bar init.
            var manaBar         = new StatusBar();
            manaBar.SetPresentationData("HUD\\parts",
                                        StatusBarTextureSources.Compute(0, 32, 32, 32, 4, 1),
                                        StatusBarBindingsSource.Create(() => 0,
                                                                       () => controller.Specialization.TotalMana(),
                                                                       () => controller.Statuses.Mana));
            manaBar.ShowText    = true;
            manaBar.Size        = new Vector2(0.8f, 0.33f);
            manaBar.TextType    = TextType.Both;
            manaBar.Position    = new Vector2(0.0f, healthBar.DisplayBounds.Bottom);
            statusWindow.Add(manaBar);

            // Focus bar init.
            var focusBar        = new StatusBar();
            focusBar.SetPresentationData("HUD\\parts",
                                         StatusBarTextureSources.Compute(0, 64, 32, 32, 4, 1),
                                         StatusBarBindingsSource.Create(() => 0,
                                                                        () => controller.Specialization.TotalFocus(),
                                                                        () => controller.Statuses.Focus));
            focusBar.ShowText   = true;
            focusBar.Size       = new Vector2(0.8f, 0.33f);
            focusBar.TextType   = TextType.Both;
            focusBar.Position   = new Vector2(0.0f, manaBar.DisplayBounds.Bottom);
            statusWindow.Add(focusBar);

            Root.Add(statusWindow);
        }

        private void CreateTargetStatusWindow()
        {
            // Target status window init.
            var statusWindow        = new Panel();
            statusWindow.Sizing     = Sizing.Percents;
            statusWindow.Size       = new Vector2(0.15f);
            statusWindow.Position   = new Vector2(HUDRenderer.Instance.CanvasSize.X * 0.5f, HUDRenderer.Instance.CanvasSize.Y * 0.015f);
            statusWindow.Element    = null;

            // Target name label.
            var targetHeader        = new Label();
            statusWindow.Add(targetHeader);

            statusWindow.Position = new Vector2(statusWindow.Position.X, targetHeader.ComputeBounds(HUDConstants.SampleTextLine).Bottom);
            targetHeader.Position = new Vector2(targetHeader.Position.X, -targetHeader.ComputeBounds(HUDConstants.SampleTextLine).Height);

            // Target health bar init.
            var healthBar       = new StatusBar();
            healthBar.SetPresentationData("HUD\\parts",
                                          StatusBarTextureSources.Compute(0, 0, 32, 32, 4, 1),
                                          StatusBarBindingsSource.Create(() => 0,
                                                                         () => controller.TargetFinder.TargetController.Specialization.TotalHealth(),
                                                                         () => controller.TargetFinder.TargetController.Statuses.Health));
            healthBar.ShowText  = true;
            healthBar.Size      = new Vector2(0.8f, 0.33f);
            healthBar.TextType  = TextType.Both;
            healthBar.Position  = new Vector2(targetHeader.Position.X, targetHeader.DisplayBounds.Bottom);
            statusWindow.Add(healthBar);

            // Target mana bar init.
            var manaBar         = new StatusBar();
            manaBar.SetPresentationData("HUD\\parts",
                                        StatusBarTextureSources.Compute(0, 32, 32, 32, 4, 1),
                                        StatusBarBindingsSource.Create(() => 0,
                                                                       () => controller.TargetFinder.TargetController.Specialization.TotalMana(),
                                                                       () => controller.TargetFinder.TargetController.Statuses.Mana));
            manaBar.ShowText    = true;
            manaBar.Size        = new Vector2(0.8f, 0.33f);
            manaBar.TextType    = TextType.Both;
            manaBar.Position    = new Vector2(healthBar.Position.X, healthBar.DisplayBounds.Bottom);
            statusWindow.Add(manaBar);

            // Target focus bar init.
            var focusBar        = new StatusBar();
            focusBar.SetPresentationData("HUD\\parts",
                                         StatusBarTextureSources.Compute(0, 64, 32, 32, 4, 1),
                                         StatusBarBindingsSource.Create(() => 0,
                                                                        () => controller.TargetFinder.TargetController.Specialization.TotalFocus(),
                                                                        () => controller.TargetFinder.TargetController.Statuses.Focus));
            focusBar.ShowText   = true;
            focusBar.Size       = new Vector2(0.8f, 0.33f);
            focusBar.TextType   = TextType.Both;
            focusBar.Position   = new Vector2(healthBar.Position.X, manaBar.DisplayBounds.Bottom);
            statusWindow.Add(focusBar);

            statusWindow.Disable();
            statusWindow.Hide();

            controller.TargetFinder.TargetChanged += (sender) =>
            {
                if (sender.Target != null)
                { 
                    targetHeader.Text = sender.TargetController.Name;
                    
                    statusWindow.Enable();
                    statusWindow.Show();

                    statusWindow.Invalidate();
                }
                else
                {
                    statusWindow.Disable();
                    statusWindow.Hide();
                }
            };

            Root.Add(statusWindow);
        }
        
        private void ConstructSubsystems()
        {
            Subsystems = new List<HUDSubsystem>()
            {
                new FloatingHUDTextManager()
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
