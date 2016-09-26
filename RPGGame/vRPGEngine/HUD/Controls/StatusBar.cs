using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        private bool showText;

        private TextType textType;

        private SpriteFont font;
        #endregion

        #region Properties
        public int Min
        {
            get
            {
                var results = 0;

                ReadProperty("Min", ref results);

                if (!HasProperty("Min")) Logger.Instance.LogWarning("status bar has no binding for min, will alwasy return 0");

                return results;
            }
        }
        public int Max
        {
            get
            {
                var results = 0;

                ReadProperty("Max", ref results);

                if (!HasProperty("Max")) Logger.Instance.LogWarning("status bar has no binding for max, will alwasy return 0");

                return results;
            }
        }
        public int Value
        {
            get
            {
                var results = 0;

                ReadProperty("Value", ref results);

                if (!HasProperty("Value")) Logger.Instance.LogWarning("status bar has no binding for value, will alwasy return 0");

                return results;
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

            RegisterProperty("ShowText", () => ShowText, (o) => ShowText = (bool)o);
            RegisterProperty("TextType", () => TextType, (o) => TextType = (TextType)o);
            RegisterProperty("Font", () => Font, (o) => Font = (SpriteFont)o);

            ValidateProperties(GetType());

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

        public void SetPresentationData(string name, StatusBarTextureSources sources, StatusBarBindingsSource bindings)
        {
            var statusBarDisplayElement = Element as IStatusBarElement;

            statusBarDisplayElement?.SetPresentationData(name, sources, bindings);

            if (bindings.Bound())
            {
                UnregisterProperty("Min");
                UnregisterProperty("Max");
                UnregisterProperty("Value");

                RegisterProperty("Min", () => bindings.Min());
                RegisterProperty("Max", () => bindings.Max());
                RegisterProperty("Value", () => bindings.Value());
            }
        }
    }
}
