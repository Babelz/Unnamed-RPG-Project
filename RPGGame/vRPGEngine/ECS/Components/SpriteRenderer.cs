﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vRPGEngine.Graphics;

namespace vRPGEngine.ECS.Components
{
    public class SpriteRenderer : Component<SpriteRenderer>
    {
        #region Fields
        private readonly Sprite sprite;
        #endregion

        #region Properties
        public Sprite Sprite
        {
            get
            {
                return sprite;
            }
        }
        public int Layer
        {
            get;
            set;
        }

        public bool Anchored
        {
            get;
            set;
        }
        public Vector2 PositionOffset
        {
            get;
            set;
        }
        #endregion

        public SpriteRenderer()
            : base()
        {
            sprite          = new Sprite();
            PositionOffset  = Vector2.Zero;
            Anchored        = true;
        }

        protected override void Initialize()
        {
            base.Initialize();

            Renderer.Instance.Add(sprite, Layer);
        }
        protected override void Deinitialize()
        {
            base.Deinitialize();

            Renderer.Instance.Remove(sprite);
        }
    }
}