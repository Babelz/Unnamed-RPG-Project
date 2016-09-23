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
using vRPGEngine.HUD.Controls;
using vRPGEngine.HUD.Elements;

namespace vRPGEngine.HUD
{
    public sealed class HUDManager : SystemManager<HUDManager>
    {
        #region Constants
        private static readonly Vector2 IconSizeInPixels = new Vector2(32.0f);
        private static readonly Vector2 IconSize         = IconSizeInPixels / HUDRenderer.Instance.CanvasSize;
        private const int BuffIconColumns                = 10;
        private const int BuffIconRows                   = 3;
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
    }
}
