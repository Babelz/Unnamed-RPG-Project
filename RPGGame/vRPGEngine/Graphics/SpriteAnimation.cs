﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;

namespace vRPGEngine.Graphics
{
    public sealed class SpriteAnimation : Renderable
    {
        #region Fields
        private readonly Rectangle[] frames;
        private readonly int frameTime;

        private int frameIndex;
        private int elasped;
        #endregion

        #region Properties
        public Texture2D Texture
        {
            get;
            set;
        }
        public Rectangle Source
        {
            get;
            set;
        }
        public Color Color
        {
            get;
            set;
        }
        public float Rotation
        {
            get;
            set;
        }
        #endregion

        public SpriteAnimation(Texture2D texture, int frameTime, Rectangle[] frames)
        {
            Debug.Assert(texture != null);
            Debug.Assert(frames != null);
            Debug.Assert(frameTime > 0.0f);

            this.frameTime = frameTime;
            this.frames = frames;

            Texture     = texture;
            Source      = new Rectangle(0, 0, texture.Width, texture.Height);
            Position    = Vector2.Zero;
            Color       = Color.White;
            Rotation    = 0.0f;
        }

        public override void Present(SpriteBatch spriteBatch, GameTime gameTime)
        {
            elasped += gameTime.ElapsedGameTime.Milliseconds;

            if (elasped > frameTime)
            {
                frameIndex++;

                elasped = 0;
            }

            if (frameIndex >= frames.Length) frameIndex = 0;

            var frame = frames[frameIndex];

            spriteBatch.Draw(Texture,
                             Position,
                             null,
                             frame,
                             Vector2.Zero,
                             Rotation,
                             Scale,
                             Color,
                             SpriteEffects.None,
                             0.0f);
        }
    }
}