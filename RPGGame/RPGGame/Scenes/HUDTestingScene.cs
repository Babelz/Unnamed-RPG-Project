﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vRPGEngine.Graphics;
using vRPGEngine.HUD;
using vRPGEngine.Scenes;

namespace RPGGame.Scenes
{
    public sealed class HUDTestingScene : Scene
    {
        public HUDTestingScene()
            :base()
        {
        }

        public override void Initialize()
        {
            var root = new Panel();
            root.Element.SetValue("Color", new Color(Color.Red, 255 / 4));
            
            HUDRenderer.Instance.Root = root;
        }
        public override void Deinitialize()
        {
        }
    }
}
