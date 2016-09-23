using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vRPGEngine.Core;
using vRPGEngine.Graphics;
using vRPGEngine.HUD.Elements;

namespace vRPGEngine.HUD.Controls
{
    [Flags()]
    public enum TextType : int
    {
        None                = 0,
        Value               = 1,
        Percentage          = 2,
        Both                = Value | Percentage
    }

    public sealed class StatusBar : Control
    {
        #region Fields
        private IDisplayElement element;

        private int min;
        private int max;
        private int value;

        private bool showText;

        private TextType textType;

        private SpriteFont font;
        #endregion

        #region Properties
        public int Min
        {
            get
            {
                return min;
            }
            set
            {
                min = value;

                NotifyPropertyChanged("Min");

                if (this.value < min) Value = min;
            }
        }
        public int Max
        {
            get
            {
                return max;
            }
            set
            {
                max = value;

                NotifyPropertyChanged("Max");

                if (this.value > max) Value = max;
            }
        }
        public int Value
        {
            get
            {
                return value;
            }
            set
            {
                this.value = MathHelper.Clamp(value, min, max);

                NotifyPropertyChanged("Value");
            }
        }

        public bool ShowText
        {
            get
            {
                return showText;
            }
            set
            {
                showText = value;

                NotifyPropertyChanged("ShowText");
            }
        }
        public TextType TextType
        {
            get
            {
                return textType;
            }
            set
            {
                textType = value;

                NotifyPropertyChanged("TextType");
            }
        }

        public IDisplayElement Element
        {
            get
            {
                return element;
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
        #endregion

        public StatusBar()
                : base()
        {
            font    = DefaultValues.DefaultFont;
            element = new StatusBarElement();

            RegisterProperty("Min", () => Min, (o) => Min = (int)o);
            RegisterProperty("Max", () => Max, (o) => Max = (int)o);
            RegisterProperty("Value", () => Value, (o) => Value = (int)o);
            RegisterProperty("ShowText", () => ShowText, (o) => ShowText = (bool)o);
            RegisterProperty("TextType", () => TextType, (o) => TextType = (TextType)o);
            RegisterProperty("Font", () => Font, (o) => Font = (SpriteFont)o);

            PropertyChanged += StatusBar_PropertyChanged;
        }

        private void StatusBar_PropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            Invalidate();
        }

        protected override void OnInvalidate()
        {
            if (Element != null) Element.Invalidate(this);
        }

        protected override void OnDraw(GameTime gameTime)
        {
            if (Element != null) HUDRenderer.Instance.Show(Element);
        }

        public void SetPresentationData(string name, StatusBarTextureSources sources)
        {
            var statusBarDisplayElement = Element as IStatusBarElement;

            statusBarDisplayElement?.SetPresentationData(name, sources);
        }
    }
}
