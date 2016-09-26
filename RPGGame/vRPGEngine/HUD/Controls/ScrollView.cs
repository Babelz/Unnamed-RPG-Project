using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vRPGEngine.Graphics;
using vRPGEngine.HUD.Elements;
using vRPGEngine.HUD.Interfaces;

namespace vRPGEngine.HUD.Controls
{
    public sealed class ScrollView : MouseControlBase
    {
        #region Fields
        private IScrollViewElement element;

        private Vector2 scroll;
        #endregion

        #region Properties
        public IScrollViewElement Element
        {
            get
            {
                return element;
            }
            set
            {
                element = value;

                NotifyPropertyChanged("Element");

                if (element != null) element.SetScroll(scroll, this);
            }
        }

        public Vector2 Overlap
        {
            get
            {
                return element == null ? Vector2.Zero : element.Overlap;
            }
        }
        public Vector2 Scroll
        {
            get
            {
                return scroll;
            }
            private set
            {
                scroll = value;

                NotifyPropertyChanged("Scroll");
            }
        }
        #endregion

        public ScrollView(IScrollViewElement element)
            : base()
        {
            Debug.Assert(element != null);

            this.element = element;

            RegisterProperty("Element", () => Element, (o) => Element = (IScrollViewElement)o);
            RegisterProperty("Overlap", () => Overlap);
            RegisterProperty("Scroll", () => Scroll, (o) => Scroll = (Vector2)o);

            ValidateProperties(GetType());

            PropertyChanged += ScrollView_PropertyChanged;
        }

        #region Event handlers
        private void ScrollView_PropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "HoverState") return;

            Invalidate();
        }
        #endregion

        protected override void OnInvalidate()
        {
            if (element != null) element.Invalidate(this);
        }

        protected override void OnDraw(GameTime gameTime)
        {
            if (element != null) HUDRenderer.Instance.Show(element);
        }

        public void ScrollVertical(float pixels)
        {
            Scroll = new Vector2(Scroll.X, MathHelper.Clamp(scroll.Y + pixels, scroll.Y, Overlap.Y));

            if (element != null) element.SetScroll(Scroll, this);
        }
        public void ScrollHorizontal(float pixels)
        {
            Scroll = new Vector2(MathHelper.Clamp(scroll.X + pixels, scroll.X, Overlap.X), scroll.Y);

            if (element != null) element.SetScroll(Scroll, this);
        }
    }
}
