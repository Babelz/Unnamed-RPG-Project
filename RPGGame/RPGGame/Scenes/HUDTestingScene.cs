using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vRPGEngine.Graphics;
using vRPGEngine.HUD;
using vRPGEngine.HUD.Controls;
using vRPGEngine.Scenes;

namespace RPGGame.Scenes
{
    public sealed class HUDTestingScene : Scene
    {
        #region Fields
        private Panel root;
        #endregion

        public HUDTestingScene()
            :base()
        {
        }

        public override void Initialize()
        {
            root = new Panel();
            root.Fill = new Color(Color.Red, 255 / 4);

            var label = new Label();
            label.Text              = "Hello, world!";
            label.AdjustTextSize    = false;
            label.Size              = new Vector2(0.25f);

            root.Add(label);

            HUDRenderer.Instance.Root = root;
        }
        public override void Deinitialize()
        {
        }

        public override void Update(GameTime gameTime)
        {
            //var downScale = 0.001f;
            //root.Size = new Vector2(root.Size.X - downScale, root.Size.Y - downScale);
        }
    }
}
