using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using vRPGEngine.Graphics;
using vRPGEngine.HUD.Elements;

namespace vRPGEngine.HUD.Controls
{
    public sealed class Panel : Control, IContentControl
    {
        #region Fields
        private readonly List<Control> children;
        
        private IDisplayElement element;

        private Color fill;
        #endregion

        #region Properties
        public IEnumerable<Control> Children
        {
            get
            {
                return children;
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

        public Color Fill
        {
            get
            {
                return fill;
            }

            set
            {
                fill = value;

                NotifyPropertyChanged("Fill");
            }
        }
        #endregion

        public Panel()
            : base()
        {
            Fill      = Color.Red;
            element   = new SolidColorFill();
            children  = new List<Control>();

            RegisterProperty("Children", () => Children);
            RegisterProperty("Element", () => Element, (o) => Element = (IDisplayElement)o);
            RegisterProperty("Fill", () => Fill, (o) => Fill = (Color)o);

            element.Invalidate(this);

            PropertyChanged += View_PropertyChanged;
        }

        #region Event handlers
        private void View_PropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            Invalidate();
        }
        #endregion

        protected override void OnInvalidate()
        {
            for (int i = 0; i < children.Count; i++) children[i].Invalidate();
            
            if (element != null) element.Invalidate(this);
        }
        protected override void OnUpdate(GameTime gameTime)
        {
            for (int i = 0; i < children.Count; i++) children[i].Update(gameTime);
        }

        protected override void OnRender(GameTime gameTime)
        {
            if (element != null) HUDRenderer.Instance.Present(element);
        }

        public void Add(Control child)
        {
            Debug.Assert(child != null);

            if (children.Contains(child)) return;

            children.Add(child);

            child.Parent = this;
        }
        public void Remove(Control child)
        {
            Debug.Assert(child != null);

            if (!children.Remove(child)) return;

            child.Parent = null;
        }

        public IEnumerable<Control> Query(Func<Control, bool> predicate)
        {
            return children.Where(predicate);
        }
    }
}
