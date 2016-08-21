using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using vRPGContent.Data.Spells;
using vRPGEngine.HUD.Elements;
using vRPGEngine.Graphics;
using Microsoft.Xna.Framework.Graphics;

namespace vRPGEngine.HUD.Controls
{
    public sealed class Icon : MouseControlBase
    {
        #region Fields
        private SpriteFont font;

        private IDisplayElement element;

        private object content;
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

        public object Content
        {
            get
            {
                return content;
            }
            set
            {
                content = value;

                NotifyPropertyChanged("Content");
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

        public Icon()
            : base()
        {
            element = new IconElement();
            font    = DefaultValues.DefaultFont;

            RegisterProperty("Element", () => Element, (o) => Element = (IconElement)o);
            RegisterProperty("Content", () => Content, (o) => Content = o);
            RegisterProperty("Font", () => Font, (o) => Font = (SpriteFont)o);

            PropertyChanged += Icon_PropertyChanged;
        }

        #region Event handlers
        private void Icon_PropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            Invalidate();
        }
        #endregion

        protected override void OnInvalidate()
        {
            if (element != null) element.Invalidate(this);
        }

        protected override void OnRender(GameTime gameTime)
        {
            if (element != null) HUDRenderer.Instance.Present(element);
        }
    }
}
