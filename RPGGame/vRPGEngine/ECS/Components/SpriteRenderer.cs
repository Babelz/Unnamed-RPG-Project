using Microsoft.Xna.Framework;
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
        private Sprite sprite;
        #endregion

        #region Properties
        public Sprite Sprite
        {
            get
            {
                return sprite;
            }
        }

        public Vector2 PositionOffset
        {
            get;
            set;
        }
        public RenderFlags Flags
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
            Flags           |= RenderFlags.Anchored;
        }

        protected override void Initialize()
        {
            Renderer.Instance.Add(sprite, sprite.Layer);
        }
        protected override void Deinitialize()
        {
            Renderer.Instance.Remove(sprite);
            
            sprite.Position     = Vector2.Zero;
            sprite.Scale        = Vector2.One;
            sprite.Origin       = Vector2.Zero;
            sprite.Visible      = true;
            sprite.Depth        = 0.0f;
            sprite.Rotation     = 0.0f;
        }
    }
}
