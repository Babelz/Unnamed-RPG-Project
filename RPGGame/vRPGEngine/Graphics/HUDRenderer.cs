using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace vRPGEngine.Graphics
{
    public sealed class DisplayInfo
    {
        public InfoLogEntry Entry;

        public int Elapsed;
    }

    public sealed class HUDRenderer : SystemManager<HUDRenderer>
    {
        #region Fields
        private readonly SpriteBatch spriteBatch;

        private int elapsed;
        #endregion

        private HUDRenderer()
            : base()
        {
            spriteBatch = new SpriteBatch(Engine.Instance.GraphicsDevice);
        }
        
        protected override void OnUpdate(GameTime gameTime)
        {
            if (GameInfoLog.Instance.Entries().Count() != 0) elapsed += gameTime.ElapsedGameTime.Milliseconds;

            spriteBatch.Begin();

            var viewport = Engine.Instance.GraphicsDevice.Viewport;

            Vector2 offset;

            offset.X = viewport.Width / 2.0f;
            offset.Y = viewport.Height / 2.0f;

            var font = DefaultValues.DefaultFont;

            foreach (var entry in GameInfoLog.Instance.Entries())
            {
                var size     = font.MeasureString(entry.Contents);
                var position = new Vector2(offset.X - size.X / 2.0f, offset.Y + size.Y);
                var color    = entry.Type == InfoLogEntryType.Message ? Color.Green : Color.Yellow;

                spriteBatch.DrawString(DefaultValues.DefaultFont, entry.Contents, position, color);
            }

            spriteBatch.End();

            if (elapsed > 2500)
            {
                GameInfoLog.Instance.Clear();

                elapsed = 0;
            }
        }
    }
}
