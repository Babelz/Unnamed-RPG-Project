using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vRPGEngine.Core;
using vRPGEngine.Graphics;

namespace vRPGEngine.HUD.Controls
{
    public abstract class Control : DependencyPropertyContainer, IPropertyChanged
    {
        #region Fields
        private bool invalidating;

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
            private set
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
            private set
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
                var position = Position + GetContainerPosition();

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
                    case Sizing.Percents: displaySize = GetContainerSize() * Size * Scale;  break;
                    default:              displaySize = Scale * Size;                                                                            break;
                }

                displaySize.X += padX;
                displaySize.Y += padY;

                return displaySize;
            }
        }
        public Rectf DisplayBounds
        {
            get
            {
                return new Rectf
                {
                    Position = Position,
                    Size     = DisplaySize
                };
            }
        }
        #endregion

        public Control()
            : base()
        {
            Name    = GetType().Name;
            Scale   = Vector2.One;
            Size    = Vector2.One;
            Enabled = true;
            Visible = true;
            Sizing  = Sizing.Percents;

            RegisterProperty("Position", () => Position, (o) => Position = (Vector2)o);
            RegisterProperty("Size", () => Size, (o) => Size = (Vector2)o);
            RegisterProperty("Scale", () => Scale, (o) => Scale = (Vector2)o);

            RegisterProperty("Padding", () => Padding, (o) => Padding = (Padding)o);
            RegisterProperty("Margin", () => Margin, (o) => Margin = (Margin)o);

            RegisterProperty("Visible", () => Visible, (o) => Visible = (bool)o);
            RegisterProperty("Enabled", () => Enabled, (o) => Enabled = (bool)o);

            RegisterProperty("Name", () => Name, (o) => Name = (string)o);
            RegisterProperty("Parent", () => Parent, (o) => Parent = (Control)o);

            RegisterProperty("Sizing", () => Sizing, (o) => Sizing = (Sizing)o);
            
            PropertyChanged += Control_PropertyChanged;
        }

        #region Event handlers
        private void Control_PropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "Parent") Invalidate();
        }
        #endregion

        protected Vector2 GetContainerSize()
        {
            return parent == null ? HUDRenderer.Instance.CanvasSize : parent.DisplaySize;
        }
        protected Vector2 GetContainerPosition()
        {
            return parent == null ? Vector2.Zero : parent.DisplayPosition;
        }

        protected void NotifyPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        protected virtual void OnInvalidate()
        {
        }

        protected virtual void OnDraw(GameTime gameTime)
        {
        }
        protected virtual void OnUpdate(GameTime gameTime)
        {
        }

        protected virtual void OnHide()
        {
        }
        protected virtual void OnShow()
        {
        }

        protected virtual void OnEnable()
        {
        }
        protected virtual void OnDisable()
        {
        }

        public void Show()
        {
            if (Visible) return;

            Visible = true;

            OnShow();
        }
        public void Hide()
        {
            if (!Visible) return;

            Visible = false;

            OnHide();
        }

        public void Disable()
        {
            if (!Enabled) return;

            Enabled = false;

            OnDisable(); 
        }
        public void Enable()
        {
            if (!Enabled) return;

            Enabled = true;

            OnEnable();
        }

        public void Invalidate()
        {
            // Control already invalidating.
            if (invalidating) return;
            
            // Supress invalidation.
            invalidating = true;

            // Invalidate only if the control is not invalidating
            // and is enabled.
            if (Enabled) OnInvalidate();

            // Allow new invalidations.
            invalidating = false;
        }

        public void Update(GameTime gameTime)
        {
            if (Enabled) OnUpdate(gameTime);
            if (Visible) OnDraw(gameTime);
        }
    }
}
