using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vRPGEngine;
using vRPGEngine.Graphics;
using vRPGEngine.HUD;
using vRPGEngine.HUD.Controls;
using vRPGEngine.HUD.Elements;
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
            root.Fill = Color.Green;

            var label = new Label();
            label.Text              = "Hello, world!";
            label.AdjustTextSize    = false;
            label.Size              = new Vector2(0.25f);
            label.Position          = new Vector2(500.0f);

            var button = new Button();
            button.Text     = "Paina minua lujaa";
            button.Position = new Vector2(1280 * 0.5f, 720 * 0.5f);
            button.Size     = new Vector2(0.25f);;

            var textElement = new TextScrollViewElement();
            var textView = new ScrollView(textElement);
            textView.Position = new Vector2(300.0f);
            textView.Size = new Vector2(0.25f);

            textElement.Text = string.Join(" ", Enumerable.Repeat("Suka blyet idi eemil nahhui", 128));
            textView.Invalidate();

            button.ButtonDown += delegate
            {
                textView.ScrollVertical(textView.DisplaySize.Y / 10.0f);
            };

            root.Add(button);
            root.Add(label);
            root.Add(textView);

            // TODO: fix..
            // HUDRenderer.Instance.Root = root;
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
