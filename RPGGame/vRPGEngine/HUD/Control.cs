using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vRPGEngine.HUD
{
    public abstract class Control : IPropertyChanged
    {
        #region Fields
        private Vector2 position;
        private Vector2 size;
        private Vector2 scale;

        private Control parent;

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
        #endregion

        public Control()
        {
        }

        #region Event handlers
        #endregion

        protected void NotifyPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public virtual void Invalidate()
        {
        }
    }
}
