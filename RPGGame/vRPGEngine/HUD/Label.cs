using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vRPGEngine.HUD
{
    public sealed class Label : Control
    {
        #region Fields        
        private IDisplayElement element;

        private SpriteFont font;

        private string text;
        #endregion

        #region Properties
        public SpriteFont Font
        {
            get
            {
                return font;
            }
            set
            {
                font = value;
            }
        }

        public string Text
        {
            get
            {
                return text;
            }
            set
            {
                text = value;
            }
        }

        public IDisplayElement Element
        {
            get
            {
                return element;
            }
            set
            {
                element = value;
            }
        }
        #endregion

        public Label()
            : base()
        {
            font = DefaultValues.DefaultFont;

            element = new TextElement();
        }
    }
}
