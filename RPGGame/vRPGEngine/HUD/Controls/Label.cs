using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vRPGEngine.Graphics;
using vRPGEngine.HUD.Elements;

namespace vRPGEngine.HUD.Controls
{
    public sealed class Label : Control
    {
        #region Fields        
        private IDisplayElement element;

        private Color textColor;
        private SpriteFont font;
        
        private string text;

        private bool adjustTextSize;
        #endregion

        #region Properties
        public Color TextColor
        {
            get
            {
                return textColor;
            }
            set
            {
                textColor = value;

                NotifyPropertyChanged("TextColor");
            }
        }
        public SpriteFont Font
        {
            get
            {
                return font;
            }
            set
            {
                font = value;

                NotifyPropertyChanged("Font");
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

                NotifyPropertyChanged("Text");
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

                NotifyPropertyChanged("Element");

                element.Invalidate(this);
            }
        }

        public bool AdjustTextSize
        {
            get
            {
                return adjustTextSize;
            }

            set
            {
                adjustTextSize = value;

                NotifyPropertyChanged("AdjustTextSize");
            }
        }
        #endregion

        public Label()
            : base()
        {
            textColor      = Color.White;
            font           = DefaultValues.DefaultFont;
            element        = new TextElement();
            text           = "vRPGEngine.Label";
            Sizing         = Sizing.Percents;
            AdjustTextSize = true;

            RegisterProperty("Font", () => Font, (o) => Font = (SpriteFont)o);
            RegisterProperty("Text", () => Text, (o) => Text = (string)o);
            RegisterProperty("Element", () => Element, (o) => Element = (IDisplayElement)o);
            RegisterProperty("TextColor", () => TextColor, (o) => TextColor = (Color)o);

            PropertyChanged += Label_PropertyChanged;
        }

        #region Properties
        private void Label_PropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            Invalidate();
        }
        #endregion

        private void AdjustText()
        {
            switch (Sizing)
            {
                case Sizing.Percents:
                    Size = (font.MeasureString(text) * Scale) / GetContainerSize();
                    break;
                case Sizing.Pixels:
                    Size = font.MeasureString(text) * Scale;
                    break;
            }
        }

        protected override void OnRender(GameTime gameTime)
        {
            if (element != null) HUDRenderer.Instance.Present(element);
        }

        protected override void OnInvalidate()
        {
            if (AdjustTextSize) AdjustText();

            if (element != null) element.Invalidate(this);
        }
    }
}
