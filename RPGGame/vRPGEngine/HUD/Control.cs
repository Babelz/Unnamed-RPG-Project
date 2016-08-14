using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vRPGEngine.Graphics;

namespace vRPGEngine.HUD
{
    public abstract class Control : IPropertyChanged
    {
        #region Fields
        private Vector2 position;
        private Vector2 size;
        private Vector2 scale;

        private Margin margin;
        private Padding padding;

        private Control parent;

        private Sizing sizing;

        private bool visible;
        private bool enabled;

        private string name;
        #endregion

        #region Events
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region Properties
        public Vector2 Position
        {
            get
            {
                return position;
            }
            set
            {
                position = value;

                NotifyPropertyChanged("Position");
            }
        }
        public Vector2 Size
        {
            get
            {
                return size;
            }
            set
            {
                size = value;
                
                NotifyPropertyChanged("Size");
            }
        }
        public Vector2 Scale
        {
            get
            {
                return scale;
            }
            set
            {
                scale = value;
                
                NotifyPropertyChanged("Scale");
            }
        }

        public Margin Margin
        {
            get
            {
                return margin;
            }
            set
            {
                margin = value;

                NotifyPropertyChanged("Margin");
            }
        }
        public Padding Padding
        {
            get
            {
                return padding;
            }
            set
            {
                padding = value;

                NotifyPropertyChanged("Padding");
            }
        }

        public bool Visible
        {
            get
            {
                return visible;
            }
            set
            {
                visible = value;

                NotifyPropertyChanged("Visible");
            }
        }
        public bool Enabled
        {
            get
            {
                return enabled;
            }
            set
            {
                enabled = value;

                NotifyPropertyChanged("Enabled");
            }
        }

        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;

                NotifyPropertyChanged("Name");
            }
        }

        public Control Parent
        {
            get
            {
                return parent;
            }
            set
            {
                parent = value;

                NotifyPropertyChanged("Parent");
            }
        }

        public Sizing Sizing
        {
            get
            {
                return sizing;
            }
            set
            {
                sizing = value;

                NotifyPropertyChanged("Sizing");
            }
        }

        public Vector2 DisplayPosition
        {
            get
            {
                var position = Position;

                position.X = margin.Left + position.X - margin.Right;
                position.Y = margin.Top + position.Y - margin.Bottom;

                return position;
            }
        }
        public Vector2 DisplaySize
        {
            get
            {
                var padX = padding.Right - padding.Left;
                var padY = padding.Bottom - padding.Top;

                Vector2 displaySize;

                switch (Sizing)
                {
                    case Sizing.Percents: displaySize = (parent == null ? HUDRenderer.Instance.CanvasSize : parent.Size) * Size * Scale;  break;
                    default:              displaySize = Scale * Size;                                                                     break;
                }

                displaySize.X += padX;
                displaySize.Y += padY;

                return displaySize;
            }
        }
        public Bounds DisplayBounds
        {
            get
            {
                return new Bounds
                {
                    X = Position.X,
                    Y = Position.Y,
                    W = Size.X,
                    H = Size.Y
                };
            }
        }
        #endregion

        public Control()
        {
            Name    = GetType().Name;
            Enabled = true;
            Visible = true;

            PropertyChanged += Control_PropertyChanged;
        }

        #region Event handlers
        private void Control_PropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "Parent") Invalidate();
        }
        #endregion

        protected void NotifyPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        protected virtual void OnInvalidate()
        {
        }

        protected virtual void OnRender(GameTime gameTime)
        {
        }
        protected virtual void OnUpdate(GameTime gameTime)
        {
        }

        public void Invalidate()
        {
            if (Enabled) OnInvalidate();
        }

        public void Update(GameTime gameTime)
        {
            if (Enabled) OnUpdate(gameTime);
            if (Visible) OnRender(gameTime);
        }
    }
}
