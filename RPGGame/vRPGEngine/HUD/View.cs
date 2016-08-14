using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using vRPGEngine.Graphics;

namespace vRPGEngine.HUD
{
    public sealed class View : Control, IContentControl
    {
        #region Fields
        private readonly List<Control> children;
        
        private IDisplayElement element;
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
            }
        }
        #endregion

        public View()
            : base()
        {
            element  = new SolidColorFill()
            {
                Color = Color.Red
            };

            children = new List<Control>();

            PropertyChanged += View_PropertyChanged;
        }

        #region Event handlers
        private void View_PropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "Position" ||
                args.PropertyName == "Size" ||
                args.PropertyName == "Scale" ||
                args.PropertyName == "Margin" ||
                args.PropertyName == "Padding" ||
                args.PropertyName == "Parent" ||
                args.PropertyName == "Sizing")
            {
                Invalidate();
            }
        }
        #endregion

        protected override void OnInvalidate()
        {
            for (int i = 0; i < children.Count; i++) children[i].Invalidate();
        }
        protected override void OnUpdate(GameTime gameTime)
        {
            if (element != null) element.Invalidate(this);

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
