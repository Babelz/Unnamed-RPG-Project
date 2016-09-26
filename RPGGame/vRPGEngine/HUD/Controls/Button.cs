using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using vRPGEngine.HUD.Elements;
using vRPGEngine.Graphics;

namespace vRPGEngine.HUD.Controls
{
    public sealed class Button : ButtonBase
    {
        #region Fields
        private IDisplayElement element;

        private string text;
        #endregion

        #region Properties
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
        #endregion

        public Button()
            : base()
        {
            Element = new DefaultButtonElement();
            Text    = "vRPGEngine.Button";

            RegisterProperty("Text", () => Text, (o) => Text = (string)o);
            RegisterProperty("Element", () => Element, (o) => Element = (IDisplayElement)o);

            ValidateProperties(GetType());

            PropertyChanged += Button_PropertyChanged;
        }

        private void Button_PropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            // TODO: check invalidations.
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
    }
}
