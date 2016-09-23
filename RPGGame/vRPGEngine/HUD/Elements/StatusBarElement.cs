using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using vRPGEngine.HUD.Controls;

namespace vRPGEngine.HUD.Elements
{
    public sealed class StatusBarElement : IStatusBarElement
    {
        #region Fields
        private StatusBarTextureSources sources;

        private Texture2D texture;

        private int min;
        private int max;
        private int value;

        private bool showText;

        private TextType textType;
        #endregion

        public StatusBarElement()
        {
        }

        public void SetPresentationData(string texture, StatusBarTextureSources sources)
        {
            this.texture = Engine.Instance.Content.Load<Texture2D>(texture);
            this.sources = sources;
        }

        public void Invalidate(Control control)
        {
            if (!control.ReadProperty("Min", ref min))      return;
            if (!control.ReadProperty("Max", ref max))      return;
            if (!control.ReadProperty("Value", ref value))  return;

            if (!control.ReadProperty("ShowText", ref showText)) showText = false;
            if (!control.ReadProperty("TextType", ref textType)) textType = TextType.None;
        }
        public void Show(GameTime gameTime, SpriteBatch spriteBatch)
        {
            throw new NotImplementedException();
        }
    }
}
